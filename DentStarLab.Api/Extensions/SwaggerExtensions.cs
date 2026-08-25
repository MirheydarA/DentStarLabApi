using Microsoft.OpenApi.Models;

namespace DentStarLab.Api.Extensions;

public static class SwaggerExtensions
{
    public static void InitSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
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
                    Description = "Enter your JWT token"
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
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                        },

                        Array.Empty<string>()
                    }
                });
        });
    }

    public static void ApplySwagger(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI();
    }
}