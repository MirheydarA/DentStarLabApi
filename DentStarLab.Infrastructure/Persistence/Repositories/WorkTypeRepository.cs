using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentStarLab.Infrastructure.Persistence.Repositories;

public class WorkTypeRepository : IWorkTypeRepository
{
    private readonly AppDbContext _context;

    public WorkTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkType?> GetByIdAsync(int id)
    {
        return await _context.WorkTypes
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<WorkType>> GetAllAsync()
    {
        return await _context.WorkTypes
            .ToListAsync();
    }

    public async Task<List<WorkType>> GetActiveAsync()
    {
        return await _context.WorkTypes
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task AddAsync(WorkType workType)
    {
        await _context.WorkTypes.AddAsync(workType);
    }

    public void Update(WorkType workType)
    {
        _context.WorkTypes.Update(workType);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}