using System.Text;
using DentStarLab.Api.Logging;
using DentStarLab.Application.Configuration;
using DentStarLab.Application.Interfaces;
using DentStarLab.Application.Services;
using DentStarLab.Infrastructure;
using DentStarLab.Infrastructure.Persistence;
using DentStarLab.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// =====================================================
// Serilog Configuration (builder-dən ƏVVƏL)
// =====================================================

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();


WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

var telegramBotToken = builder.Configuration["Telegram:BotToken"];
var telegramChatId = builder.Configuration["Telegram:ChatId"];

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341");

if (!string.IsNullOrWhiteSpace(telegramBotToken) &&
    !string.IsNullOrWhiteSpace(telegramChatId))
{
    loggerConfig = loggerConfig.WriteTo.Sink(
        new TelegramLogSink(telegramBotToken, telegramChatId),
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error);
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();

// =====================================================
// Serilog-u host-a bağla
// =====================================================

builder.Host.UseSerilog(); // <-- YENİ


// =====================================================
// Infrastructure
// =====================================================

builder.Services.AddInfrastructure(
    builder.Configuration);


// =====================================================
// JWT Settings
// =====================================================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));


// =====================================================
// Application Services
// =====================================================

builder.Services.AddScoped<WorkTypeService>();
builder.Services.AddScoped<WorkService>();
builder.Services.AddScoped<DoctorService>();

builder.Services.AddScoped<PasswordService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<DoctorPortalService>();


// =====================================================
// Authentication
// =====================================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtSettings>()!;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,

                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key))
            };
    });


// =====================================================
// Authorization
// =====================================================

builder.Services.AddAuthorization();


// =====================================================
// CORS
// =====================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.AllowAnyOrigin()
            .WithOrigins(
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// =====================================================
// Controllers
// =====================================================

builder.Services.AddControllers();


// =====================================================
// Swagger
// =====================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter your JWT token"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});


// =====================================================
// Build Application
// =====================================================

var app = builder.Build();


// =====================================================
// Serilog Request Logging (hər HTTP sorğusunu avtomatik logla)
// =====================================================

app.UseSerilogRequestLogging(); // <-- YENİ


// =====================================================
// Development
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// =====================================================
// HTTPS
// =====================================================

app.UseHttpsRedirection();


// =====================================================
// CORS
// IMPORTANT: Must be before Authentication
// =====================================================

app.UseCors("Frontend");


// =====================================================
// Authentication
// =====================================================

app.UseAuthentication();


// =====================================================
// Authorization
// =====================================================

app.UseAuthorization();


// =====================================================
// Controllers
// =====================================================

app.MapControllers();


// =====================================================
// Database Seed
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await DataSeeder.SeedAsync(context);
}


// =====================================================
// Run
// =====================================================

try
{
    Log.Information("DentStarLab API başladılır...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Tətbiq başlama zamanı gözlənilməz şəkildə dayandı");
}
finally
{
    Log.CloseAndFlush();
}