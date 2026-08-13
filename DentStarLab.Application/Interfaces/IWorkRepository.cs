using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Interfaces;

public interface IWorkRepository
{
    Task AddAsync(Work work);
    Task<List<Work>> GetAllAsync();
    Task<Work?> GetByIdAsync(int id);
    Task UpdateAsync(Work work);
    Task DeleteAsync(Work work);
    Task SaveChangesAsync();
}
