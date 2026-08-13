using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using DentStarLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DentStarLab.Infrastructure.Persistence.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
    }

    public async Task<List<Doctor>> GetAllAsync()
    {
        return await _context.Doctors.Where( x => x.IsActive).ToListAsync();
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors.Where( x => x.IsActive).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}