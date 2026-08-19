using DentStarLab.Application.DTOs.WorkTypes;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class WorkTypeService
{
    private readonly IWorkTypeRepository _repository;

    public WorkTypeService(
        IWorkTypeRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // CREATE
    // =====================================================

    public async Task<WorkTypeDto> CreateAsync(
        WorkTypeCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new Exception(
                "Work type name is required.");

        if (dto.PricePerTooth <= 0)
            throw new Exception(
                "Price must be greater than zero.");

        var workType = new WorkType
        {
            Name = dto.Name.Trim(),

            PricePerTooth =
                dto.PricePerTooth,

            IsActive = true
        };

        await _repository.AddAsync(workType);

        await _repository.SaveChangesAsync();

        return MapToDto(workType);
    }

    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<List<WorkTypeDto>> GetAllAsync()
    {
        var workTypes =
            await _repository.GetAllAsync();

        return workTypes
            .Select(MapToDto)
            .ToList();
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<WorkTypeDto?> GetByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var workType =
            await _repository.GetByIdAsync(id);

        if (workType == null)
            return null;

        return MapToDto(workType);
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<bool> UpdateAsync(
        int id,
        WorkTypeUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (id <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new Exception(
                "Work type name is required.");

        if (dto.PricePerTooth <= 0)
            throw new Exception(
                "Price must be greater than zero.");

        var workType =
            await _repository.GetByIdAsync(id);

        if (workType == null)
            return false;

        workType.Name =
            dto.Name.Trim();

        workType.PricePerTooth =
            dto.PricePerTooth;

        await _repository.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // DEACTIVATE
    // =====================================================

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            return false;

        var workType =
            await _repository.GetByIdAsync(id);

        if (workType == null)
            return false;

        // Physical delete yoxdur.
        // Sadəcə deaktiv edirik.

        workType.IsActive = false;

        await _repository.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // MAPPING
    // =====================================================

    private static WorkTypeDto MapToDto(
        WorkType workType)
    {
        return new WorkTypeDto
        {
            Id =
                workType.Id,

            Name =
                workType.Name,

            PricePerTooth =
                workType.PricePerTooth,

            IsActive =
                workType.IsActive
        };
    }
}