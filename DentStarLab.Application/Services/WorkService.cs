using DentStarLab.Application.DTOs.Common;
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

    // =====================================================
    // CREATE
    // =====================================================

    public async Task<WorkDto> CreateAsync(WorkCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (string.IsNullOrWhiteSpace(dto.PatientName))
            throw new Exception("Patient name is required.");

        if (dto.DoctorId <= 0)
            throw new Exception("Doctor is required.");

        if (dto.Items == null || dto.Items.Count == 0)
            throw new Exception(
                "At least one work type must be selected.");

        // =================================================
        // Doctor yoxlanışı
        // =================================================

        var doctor =
            await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception("Doctor not found.");

        if (!doctor.IsActive)
            throw new Exception("Doctor is not active.");

        // =================================================
        // Work yarat
        // =================================================

        var work = new Work
        {
            DoctorId = doctor.Id,
            PatientName = dto.PatientName.Trim(),
            WorkDate = dto.WorkDate
        };

        // =================================================
        // WorkItems yarat
        // =================================================

        foreach (var itemDto in dto.Items)
        {
            if (itemDto.WorkTypeId <= 0)
            {
                throw new Exception(
                    "Invalid work type.");
            }

            if (itemDto.Quantity <= 0)
            {
                throw new Exception(
                    "Quantity must be greater than zero.");
            }

            var workType =
                await _workTypeRepository
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

            // =================================================
            // Vacib:
            //
            // Frontend price göndərmir.
            //
            // Backend WorkType-dan qiyməti götürür.
            // =================================================

            var unitPrice = workType.PricePerTooth;

            var totalAmount =
                unitPrice * itemDto.Quantity;

            var workItem = new WorkItem
            {
                WorkTypeId = workType.Id,

                ToothCount = itemDto.Quantity,

                // Historical price
                UnitPrice = unitPrice,

                // Historical total
                TotalAmount = totalAmount
            };

            work.Items.Add(workItem);
        }

        // =================================================
        // Save
        // =================================================

        await _workRepository.AddAsync(work);

        await _workRepository.SaveChangesAsync();

        return MapToDto(work);
    }

    // =====================================================
    // GET PAGED (filter + pagination)
    // =====================================================

    public async Task<PagedResultDto<WorkDto>> GetPagedAsync(WorkQueryDto query)
    {
        var (works, totalCount) =
            await _workRepository.GetPagedAsync(query);

        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        return new PagedResultDto<WorkDto>
        {
            Items = works.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<WorkDto?> GetByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var work =
            await _workRepository.GetByIdAsync(id);

        if (work == null)
            return null;

        return MapToDto(work);
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<bool> UpdateAsync(
        int id,
        WorkUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (id <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(dto.PatientName))
            throw new Exception(
                "Patient name is required.");

        if (dto.DoctorId <= 0)
            throw new Exception(
                "Doctor is required.");

        if (dto.Items == null || dto.Items.Count == 0)
            throw new Exception(
                "At least one work type must be selected.");

        // =================================================
        // Work
        // =================================================

        var work =
            await _workRepository.GetByIdAsync(id);

        if (work == null)
            return false;

        // =================================================
        // Doctor
        // =================================================

        var doctor =
            await _doctorRepository
                .GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception(
                "Doctor not found.");

        if (!doctor.IsActive)
            throw new Exception(
                "Doctor is not active.");

        // =================================================
        // Work məlumatlarını yenilə
        // =================================================

        work.DoctorId = doctor.Id;

        work.PatientName =
            dto.PatientName.Trim();

        work.WorkDate =
            dto.WorkDate;

        // =================================================
        // Mövcud WorkItems-ları təmizlə
        // =================================================

        work.Items.Clear();

        // =================================================
        // Yeni WorkItems
        // =================================================

        foreach (var itemDto in dto.Items)
        {
            if (itemDto.WorkTypeId <= 0)
            {
                throw new Exception(
                    "Invalid work type.");
            }

            if (itemDto.Quantity <= 0)
            {
                throw new Exception(
                    "Quantity must be greater than zero.");
            }

            var workType =
                await _workTypeRepository
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

            // =================================================
            // Cari qiymət
            // =================================================

            var unitPrice =
                workType.PricePerTooth;

            var totalAmount =
                unitPrice * itemDto.Quantity;

            var workItem = new WorkItem
            {
                WorkTypeId = workType.Id,

                ToothCount =
                    itemDto.Quantity,

                UnitPrice =
                    unitPrice,

                TotalAmount =
                    totalAmount
            };

            work.Items.Add(workItem);
        }

        // =================================================
        // Save
        // =================================================

        await _workRepository.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            return false;

        var work =
            await _workRepository.GetByIdAsync(id);

        if (work == null)
            return false;

        await _workRepository.DeleteAsync(work);

        await _workRepository.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // MAPPING
    // =====================================================

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

            DoctorName = $"{work.Doctor.Name} {work.Doctor.Surname}",

            PatientName = work.PatientName,

            WorkDate = work.WorkDate,

            CreatedAt = work.CreatedAt,

            Items = items,

            TotalPrice =
                items.Sum(x => x.TotalPrice)
        };
    }
}