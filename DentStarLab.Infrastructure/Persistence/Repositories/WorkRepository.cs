using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentStarLab.Infrastructure.Persistence.Repositories;

public class WorkRepository : IWorkRepository
{
    private readonly AppDbContext _context;

    public WorkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Work work)
    {
        await _context.Works.AddAsync(work);
    }

    public async Task<List<Work>> GetAllAsync()
    {
        return await _context.Works
            .Include(x => x.Items)
                .ThenInclude(x => x.WorkType)
            .Include(x => x.Doctor)
            .ToListAsync();
    }

    public async Task<Work?> GetByIdAsync(int id)
    {
        return await _context.Works
            .Include(x => x.Items)
                .ThenInclude(x => x.WorkType)
            .Include(x => x.Doctor)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Work work)
    {
        _context.Works.Update(work);

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Work work)
    {
        _context.Works.Remove(work);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}