using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task UpdateAsync(RefreshToken refreshToken);
    Task RevokeAllForUserAsync(int userId);
}
