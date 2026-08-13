using DentStarLab.Application.DTOs.Works;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class WorkService
{
    private readonly IWorkRepository _workRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IWorkTypeRepository _workTypeRepository;

    public WorkService(
        IWorkRepository workRepository,
        IDoctorRepository doctorRepository,
        IWorkTypeRepository workTypeRepository)
    {
        _workRepository = workRepository;
        _doctorRepository = doctorRepository;
        _workTypeRepository = workTypeRepository;
    }

    public async Task<WorkDto> CreateAsync(WorkCreateDto dto)
    {
        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception("Doctor not found.");

        if (!doctor.IsActive)
            throw new Exception("Doctor is not active.");

        var work = new Work
        {
            DoctorId = dto.DoctorId,
            PatientName = dto.PatientName,
            WorkDate = dto.WorkDate
        };

        foreach (var itemDto in dto.Items)
        {
            if (itemDto.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero.");

            var workType = await _workTypeRepository
                .GetByIdAsync(itemDto.WorkTypeId);

            if (workType == null)
            {
                throw new Exception(
                    $"WorkType with id {itemDto.WorkTypeId} not found.");
            }

            if (!workType.IsActive)
            {
                throw new Exception(
                    $"WorkType '{workType.Name}' is not active.");
            }

            var workItem = new WorkItem
            {
                WorkTypeId = workType.Id,

                ToothCount = itemDto.Quantity,

                UnitPrice = workType.PricePerTooth,

                TotalAmount =
                    workType.PricePerTooth * itemDto.Quantity
            };

            work.Items.Add(workItem);
        }

        await _workRepository.AddAsync(work);

        await _workRepository.SaveChangesAsync();

        return MapToDto(work);
    }

    public async Task<List<WorkDto>> GetAllAsync()
    {
        var works = await _workRepository.GetAllAsync();

        return works
            .Select(MapToDto)
            .ToList();
    }

    public async Task<WorkDto?> GetByIdAsync(int id)
    {
        var work = await _workRepository.GetByIdAsync(id);

        if (work == null)
            return null;

        return MapToDto(work);
    }

    public async Task<bool> UpdateAsync(int id, WorkUpdateDto dto)
    {
        var work = await _workRepository.GetByIdAsync(id);

        if (work == null)
            return false;

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception("Doctor not found.");

        if (!doctor.IsActive)
            throw new Exception("Doctor is not active.");

        work.DoctorId = dto.DoctorId;
        work.PatientName = dto.PatientName;
        work.WorkDate = dto.WorkDate;

        work.Items.Clear();

        foreach (var itemDto in dto.Items)
        {
            if (itemDto.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero.");

            var workType = await _workTypeRepository
                .GetByIdAsync(itemDto.WorkTypeId);

            if (workType == null)
            {
                throw new Exception(
                    $"WorkType with id {itemDto.WorkTypeId} not found.");
            }

            if (!workType.IsActive)
            {
                throw new Exception(
                    $"WorkType '{workType.Name}' is not active.");
            }

            var workItem = new WorkItem
            {
                WorkTypeId = workType.Id,

                ToothCount = itemDto.Quantity,

                UnitPrice = workType.PricePerTooth,

                TotalAmount =
                    workType.PricePerTooth * itemDto.Quantity
            };

            work.Items.Add(workItem);
        }

        await _workRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var work = await _workRepository.GetByIdAsync(id);

        if (work == null)
            return false;

        await _workRepository.DeleteAsync(work);

        await _workRepository.SaveChangesAsync();

        return true;
    }

    private static WorkDto MapToDto(Work work)
    {
        var items = work.Items
            .Select(item => new WorkItemDto
            {
                Id = item.Id,

                WorkTypeId = item.WorkTypeId,

                WorkTypeName = item.WorkType.Name,

                Quantity = item.ToothCount,

                UnitPrice = item.UnitPrice,

                TotalPrice = item.TotalAmount
            })
            .ToList();

        return new WorkDto
        {
            Id = work.Id,
            DoctorId = work.DoctorId,
            PatientName = work.PatientName,
            WorkDate = work.WorkDate,
            CreatedAt = work.CreatedAt,

            Items = items,

            TotalPrice = items.Sum(x => x.TotalPrice)
        };
    }
}