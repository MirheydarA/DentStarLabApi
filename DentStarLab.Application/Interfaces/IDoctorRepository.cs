using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Interfaces;

public interface IDoctorRepository
{
    Task AddAsync(Doctor doctor);
    Task<List<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}