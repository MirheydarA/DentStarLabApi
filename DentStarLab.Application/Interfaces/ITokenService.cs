using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}
