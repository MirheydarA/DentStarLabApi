using DentStarLab.Infrastructure.Persistence;
using DentStarLab.Infrastructure.Persistence.Seed;

namespace DentStarLab.Api.Extensions;

public static class DatabaseSeedExtensions
{
    public static async Task ApplyDatabaseSeedAsync(this WebApplication app)
    {
        using IServiceScope? scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await DataSeeder.SeedAsync(context);
    }
}