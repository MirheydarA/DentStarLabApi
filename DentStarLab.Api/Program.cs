using DentStarLab.Api.Extensions;
using DentStarLab.Application.Configuration;
using DentStarLab.Infrastructure;
using Serilog;


WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.InitMvc();
builder.InitLog();
builder.Services.InitSwagger();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// =====================================================
// Application Services
// =====================================================

builder.Services.AddApplicationServices();


builder.Services.InitAuthentication(builder.Configuration);
builder.Services.InitAuthorization();


// =====================================================
// Build Application
// =====================================================

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.ApplyCors();             
app.ApplySwagger();
app.ApplyScalar();
app.ApplyAuthentication();
app.ApplyAuthorization();
app.MapControllers();         
await app.ApplyDatabaseSeedAsync();

app.Run();