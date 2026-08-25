namespace DentStarLab.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection InitAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }

    public static WebApplication ApplyAuthorization(this WebApplication app)
    {
        app.UseAuthorization();
        return app;
    }
}