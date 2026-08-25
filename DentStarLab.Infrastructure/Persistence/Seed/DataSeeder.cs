using DentStarLab.Domain.Entities;
using DentStarLab.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DentStarLab.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(
        AppDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var passwordHasher = new PasswordHasher<User>();

        var admin = new User
        {
            FullName = "Main Technician",
            Email = "admin@dentstarlab.com",
            Role = UserRole.Admin,
            IsActive = true
        };

        admin.PasswordHash = passwordHasher.HashPassword(
            admin,
            "admin1212");

        await context.Users.AddAsync(admin);

        await context.SaveChangesAsync();
    }
}