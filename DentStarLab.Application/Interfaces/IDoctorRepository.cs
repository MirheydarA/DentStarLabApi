using DentStarLab.Domain.Entities;
using DentStarLab.Application.DTOs.Doctors;

namespace DentStarLab.Application.Interfaces;

public interface IDoctorRepository
{
    Task AddAsync(Doctor doctor);
    Task<List<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(int id);
    Task SaveChangesAsync();
    Task<List<DoctorBalanceDto>> GetBalancesAsync();
    Task<DoctorBalanceDto?> GetBalanceByIdAsync(int doctorId);
    Task<Doctor?> GetByAccessTokenAsync(Guid accessToken);
}