using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using DentStarLab.Infrastructure.Persistence;
using DentStarLab.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DentStarLab.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IWorkTypeRepository, WorkTypeRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IWorkRepository, WorkRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}