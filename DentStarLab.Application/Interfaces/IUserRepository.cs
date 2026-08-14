using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task SaveChangesAsync();
}
