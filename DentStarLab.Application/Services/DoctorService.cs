using DentStarLab.Application.DTOs.Doctors;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _repository;
    private readonly IWorkRepository _workRepository;

    public DoctorService(IDoctorRepository repository, IWorkRepository workRepository)
    {
        _repository = repository;
        _workRepository = workRepository;
    }

    public async Task<DoctorDto> CreateAsync(
        DoctorCreateDto dto)
    {
        var doctor = new Doctor
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Phone = dto.Phone,
            Email = dto.Email,
            IsActive = true
        };

        await _repository.AddAsync(doctor);

        await _repository.SaveChangesAsync();

        return MapToDto(doctor);
    }

    public async Task<List<DoctorDto>> GetAllAsync()
    {
        var doctors = await _repository.GetAllAsync();

        return doctors.Select(MapToDto).ToList();
    }

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
            return null;

        return MapToDto(doctor);
    }

    public async Task<bool> UpdateAsync(int id, DoctorUpdateDto dto)
    {
        Doctor? doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
            return false;

        doctor.Name = dto.Name;
        doctor.Surname = dto.Surname;
        doctor.Phone = dto.Phone;
        doctor.Email = dto.Email;

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
            return false;

        doctor.IsActive = false;

        await _repository.SaveChangesAsync();

        return true;
    }

    private static DoctorDto MapToDto(Doctor doctor)
    {
        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Surname = doctor.Surname,
            Phone = doctor.Phone,
            Email = doctor.Email,
            IsActive = doctor.IsActive,
            CreatedAt = doctor.CreatedAt,
            AccessToken = doctor.AccessToken
        };
    }
    public async Task<List<DoctorBalanceDto>> GetBalancesAsync()
    {
        return await _repository.GetBalancesAsync();
    }

    public async Task<DoctorBalanceDto?> GetBalanceByIdAsync(int doctorId)
    {
        return await _repository.GetBalanceByIdAsync(doctorId);
    }

    public async Task<List<DoctorDto>> GetFrequentAsync(int days = 90, int top = 5)
    {
        var doctorIds = await _workRepository.GetFrequentDoctorIdsAsync(days, top);

        var allDoctors = await _repository.GetAllAsync();

        var doctorMap = allDoctors.ToDictionary(d => d.Id);

        return doctorIds
            .Where(id => doctorMap.ContainsKey(id))
            .Select(id => MapToDto(doctorMap[id]))
            .ToList();
    }
}