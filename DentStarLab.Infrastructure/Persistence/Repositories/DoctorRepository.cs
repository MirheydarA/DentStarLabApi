using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using DentStarLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DentStarLab.Application.DTOs.Doctors;

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


    public async Task<List<DoctorBalanceDto>> GetBalancesAsync()
    {
        var doctors = await _context.Doctors
            .Where(d => d.IsActive)
            .ToListAsync();

        var result = new List<DoctorBalanceDto>();

        foreach (var doctor in doctors)
        {
            var totalWorks = await _context.WorkItems
                .Where(wi => wi.Work.DoctorId == doctor.Id)
                .SumAsync(wi => wi.TotalAmount);

            var totalPaid = await _context.Payments
                .Where(p => p.DoctorId == doctor.Id)
                .SumAsync(p => p.Amount);

            result.Add(new DoctorBalanceDto
            {
                DoctorId = doctor.Id,
                DoctorName = $"{doctor.Name} {doctor.Surname}",
                TotalWorksAmount = totalWorks,
                TotalPaidAmount = totalPaid,
                Balance = totalWorks - totalPaid
            });
        }

        return result.OrderByDescending(d => d.Balance).ToList();
    }

    public async Task<DoctorBalanceDto?> GetBalanceByIdAsync(int doctorId)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == doctorId);

        if (doctor == null)
            return null;

        var totalWorks = await _context.WorkItems
            .Where(wi => wi.Work.DoctorId == doctorId)
            .SumAsync(wi => wi.TotalAmount);

        var totalPaid = await _context.Payments
            .Where(p => p.DoctorId == doctorId)
            .SumAsync(p => p.Amount);

        return new DoctorBalanceDto
        {
            DoctorId = doctor.Id,
            DoctorName = $"{doctor.Name} {doctor.Surname}",
            TotalWorksAmount = totalWorks,
            TotalPaidAmount = totalPaid,
            Balance = totalWorks - totalPaid
        };
    }
    public async Task<Doctor?> GetByAccessTokenAsync(Guid accessToken)
    {
        return await _context.Doctors
            .FirstOrDefaultAsync(x => x.AccessToken == accessToken);
    }
}