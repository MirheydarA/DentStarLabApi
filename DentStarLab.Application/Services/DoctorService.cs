using DentStarLab.Application.DTOs.Doctors;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _repository;

    public DoctorService(IDoctorRepository repository)
    {
        _repository = repository;
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
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
            return false;

        doctor.Name = dto.Name;
        doctor.Surname = dto.Surname;
        doctor.Phone = dto.Phone;
        doctor.Email = dto.Email;
        doctor.IsActive = dto.IsActive;

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
            CreatedAt = doctor.CreatedAt
        };
    }
}