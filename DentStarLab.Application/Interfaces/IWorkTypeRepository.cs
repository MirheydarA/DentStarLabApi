using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Interfaces;
public interface IWorkTypeRepository
{
    Task<WorkType?> GetByIdAsync(int id);

    Task<List<WorkType>> GetAllAsync();

    Task<List<WorkType>> GetActiveAsync();

    Task AddAsync(WorkType workType);

    void Update(WorkType workType);
    
    Task SaveChangesAsync();
}