using System.Text;
using DentStarLab.Application.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DentStarLab.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection InitAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration
                    .GetSection("Jwt")
                    .Get<JwtSettings>();

                if (jwtSettings is null)
                    throw new InvalidOperationException("JWT settings configuration is missing.");
                

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

        return services;
    }

    public static WebApplication ApplyAuthentication(this WebApplication app)
    {
        app.UseAuthentication();

        return app;
    }
}