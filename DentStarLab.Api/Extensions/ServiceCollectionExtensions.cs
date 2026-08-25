using DentStarLab.Application.Interfaces;
using DentStarLab.Application.Services;

namespace DentStarLab.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<WorkTypeService>();
        services.AddScoped<WorkService>();
        services.AddScoped<DoctorService>();
        services.AddScoped<PasswordService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<DashboardService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<DoctorPortalService>();

        return services;
    }
}