using DentStarLab.Application.DTOs.Common;
using DentStarLab.Application.DTOs.Payments;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class PaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IDoctorRepository _doctorRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IDoctorRepository doctorRepository)
    {
        _paymentRepository = paymentRepository;
        _doctorRepository = doctorRepository;
    }

    // =====================================================
    // CREATE
    // =====================================================

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.DoctorId <= 0)
            throw new Exception("Doctor is required.");

        if (dto.Amount <= 0)
            throw new Exception("Amount must be greater than zero.");

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception("Doctor not found.");

        if (!doctor.IsActive)
            throw new Exception("Doctor is not active.");

        var payment = new Payment
        {
            DoctorId = doctor.Id,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate == default
                ? DateTime.UtcNow
                : dto.PaymentDate,
            Note = dto.Note?.Trim()
        };

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        payment.Doctor = doctor;

        return MapToDto(payment);
    }


    public async Task<bool> UpdateAsync(int id, PaymentUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (id <= 0)
            return false;

        if (dto.DoctorId <= 0)
            throw new Exception("Doctor is required.");

        if (dto.Amount <= 0)
            throw new Exception("Amount must be greater than zero.");

        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment == null)
            return false;

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception("Doctor not found.");

        if (!doctor.IsActive)
            throw new Exception("Doctor is not active.");

        payment.DoctorId = doctor.Id;
        payment.Amount = dto.Amount;
        payment.PaymentDate = dto.PaymentDate;
        payment.Note = dto.Note?.Trim();

        await _paymentRepository.UpdateAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // GET PAGED
    // =====================================================

    public async Task<PagedResultDto<PaymentDto>> GetPagedAsync(PaymentQueryDto query)
    {
        var (payments, totalCount) = await _paymentRepository.GetPagedAsync(query);

        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        return new PagedResultDto<PaymentDto>
        {
            Items = payments.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            return false;

        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment == null)
            return false;

        await _paymentRepository.DeleteAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // MAPPING
    // =====================================================

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            DoctorId = payment.DoctorId,
            DoctorName = $"{payment.Doctor.Name} {payment.Doctor.Surname}",
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            Note = payment.Note,
            CreatedAt = payment.CreatedAt
        };
    }
}