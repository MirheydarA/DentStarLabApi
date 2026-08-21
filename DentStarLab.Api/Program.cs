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
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

// =====================================================
// Serilog Configuration
// =====================================================

var builder = WebApplication.CreateBuilder(args);

var telegramBotToken =
    builder.Configuration["Telegram:BotToken"];

var telegramChatId =
    builder.Configuration["Telegram:ChatId"];

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override(
        "Microsoft",
        LogEventLevel.Warning)
    .MinimumLevel.Override(
        "Microsoft.EntityFrameworkCore",
        LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console();

if (!string.IsNullOrWhiteSpace(telegramBotToken) &&
    !string.IsNullOrWhiteSpace(telegramChatId))
{
    loggerConfig = loggerConfig.WriteTo.Sink(
        new TelegramLogSink(
            telegramBotToken,
            telegramChatId),
        restrictedToMinimumLevel: LogEventLevel.Error);
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();

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
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtSettings>();

        if (jwtSettings is null)
        {
            throw new InvalidOperationException(
                "JWT settings configuration is missing.");
        }

        if (string.IsNullOrWhiteSpace(jwtSettings.Key))
        {
            throw new InvalidOperationException(
                "JWT Key is missing.");
        }

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    jwtSettings.Issuer,

                ValidAudience =
                    jwtSettings.Audience,

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

var frontendUrl =
    builder.Configuration["FrontendUrl"];

if (string.IsNullOrWhiteSpace(frontendUrl))
{
    throw new InvalidOperationException(
        "FrontendUrl configuration is missing.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(frontendUrl)
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
// Serilog Request Logging
// =====================================================

app.UseSerilogRequestLogging();

// =====================================================
// Swagger
// =====================================================

// Temporary: enabled for production deployment/testing.
// We can protect or disable it later.

app.UseSwagger();

app.UseSwaggerUI();


// =====================================================
// Scalar API Reference
// =====================================================

app.MapScalarApiReference(options =>
{
    options
        .WithOpenApiRoutePattern("/swagger/v1/swagger.json")
        .WithTitle("DentStarLab API")
        .WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

// =====================================================
// HTTPS
// =====================================================

app.UseHttpsRedirection();

// =====================================================
// CORS
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

// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider
//         .GetRequiredService<AppDbContext>();

//     await DataSeeder.SeedAsync(context);
// }

// =====================================================
// Application Start
// =====================================================

try
{
    Log.Information(
        "DentStarLab API başladılır...");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "Tətbiq başlama zamanı gözlənilməz şəkildə dayandı");
}
finally
{
    Log.CloseAndFlush();
}