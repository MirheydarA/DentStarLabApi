using DentStarLab.Application.DTOs.WorkTypes;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class WorkTypeService
{
    private readonly IWorkTypeRepository _repository;

    public WorkTypeService(IWorkTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkTypeDto> CreateAsync(WorkTypeCreateDto dto)
    {
        var workType = new WorkType
        {
            Name = dto.Name,
            PricePerTooth = dto.PricePerTooth,
            IsActive = true
        };

        await _repository.AddAsync(workType);
        await _repository.SaveChangesAsync();

        return MapToDto(workType);
    }

    public async Task<List<WorkTypeDto>> GetAllAsync()
    {
        var workTypes = await _repository.GetAllAsync();

        return workTypes
            .Select(MapToDto)
            .ToList();
    }

    public async Task<WorkTypeDto?> GetByIdAsync(int id)
    {
        var workType = await _repository.GetByIdAsync(id);

        if (workType == null)
            return null;

        return MapToDto(workType);
    }

    public async Task<bool> UpdateAsync(
        int id,
        WorkTypeUpdateDto dto)
    {
        var workType = await _repository.GetByIdAsync(id);

        if (workType == null)
            return false;

        workType.Name = dto.Name;
        workType.PricePerTooth = dto.PricePerTooth;
        workType.IsActive = dto.IsActive;

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var workType = await _repository.GetByIdAsync(id);

        if (workType == null)
            return false;

        workType.IsActive = false;

        await _repository.SaveChangesAsync();

        return true;
    }

    private static WorkTypeDto MapToDto(WorkType workType)
    {
        return new WorkTypeDto
        {
            Id = workType.Id,
            Name = workType.Name,
            PricePerTooth = workType.PricePerTooth,
            IsActive = workType.IsActive
        };
    }
}