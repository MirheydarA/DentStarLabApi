using Scalar.AspNetCore;

namespace DentStarLab.Api.Extensions;

public static class ScalarExtensions
{
    public static WebApplication ApplyScalar(this WebApplication app)
    {
        app.MapScalarApiReference(options =>
        {
            options
                .WithOpenApiRoutePattern("/swagger/v1/swagger.json")
                .WithTitle("DentStarLab API")
                .WithTheme(ScalarTheme.Purple)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}