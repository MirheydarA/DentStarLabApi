using DentStarLab.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DentStarLab.Application.Services;

public class PasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool Verify(
        User user,
        string hashedPassword,
        string password)
    {
        var result = _hasher.VerifyHashedPassword(
            user,
            hashedPassword,
            password);

        return result == PasswordVerificationResult.Success;
    }
}