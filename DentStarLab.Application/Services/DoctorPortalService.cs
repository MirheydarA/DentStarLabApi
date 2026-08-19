using DentStarLab.Application.DTOs.DoctorPortal;
using DentStarLab.Application.Interfaces;

namespace DentStarLab.Application.Services;

public class DoctorPortalService
{
    private readonly IDoctorRepository _doctorRepository;

    private readonly IWorkRepository _workRepository;

    private readonly IPaymentRepository _paymentRepository;

    public DoctorPortalService(
        IDoctorRepository doctorRepository,
        IWorkRepository workRepository,
        IPaymentRepository paymentRepository)
    {
        _doctorRepository = doctorRepository;

        _workRepository = workRepository;

        _paymentRepository = paymentRepository;
    }

    // =========================================================
    // SUMMARY
    // =========================================================

    public async Task<DoctorPortalSummaryDto?> GetSummaryAsync(
        Guid accessToken)
    {
        var doctor = await _doctorRepository
            .GetByAccessTokenAsync(accessToken);

        if (doctor == null)
            return null;

        var now = DateTime.Now;

        var monthStart = new DateTime(
            now.Year,
            now.Month,
            1);

        var nextMonthStart =
            monthStart.AddMonths(1);

        var totalWorkAmount =
            await _workRepository
                .GetDoctorTotalWorkAmountAsync(
                    doctor.Id);

        var totalPaymentAmount =
            await _paymentRepository
                .GetDoctorTotalPaymentAmountAsync(
                    doctor.Id);

        var currentMonthWorkAmount =
            await _workRepository
                .GetDoctorCurrentMonthWorkAmountAsync(
                    doctor.Id,
                    monthStart,
                    nextMonthStart);

        var currentMonthPaymentAmount =
            await _paymentRepository
                .GetDoctorCurrentMonthPaymentAmountAsync(
                    doctor.Id,
                    monthStart,
                    nextMonthStart);

        return new DoctorPortalSummaryDto
        {
            DoctorName =
                $"{doctor.Name} {doctor.Surname}".Trim(),

            CurrentBalance =
                totalWorkAmount - totalPaymentAmount,

            CurrentMonthWorkAmount =
                currentMonthWorkAmount,

            CurrentMonthPaymentAmount =
                currentMonthPaymentAmount,

            CurrentMonthStart =
                monthStart,

            CurrentMonthEnd =
                nextMonthStart.AddDays(-1)
        };
    }

    // =========================================================
    // WORKS
    // =========================================================

    public async Task<
        DoctorPortalPagedResultDto<DoctorPortalWorkDto>?
    > GetWorksAsync(
        Guid accessToken,
        DoctorPortalWorkFilterDto filter)
    {
        var doctor = await _doctorRepository
            .GetByAccessTokenAsync(accessToken);

        if (doctor == null)
            return null;

        var result =
            await _workRepository
                .GetDoctorPortalWorksAsync(
                    doctor.Id,
                    filter);

        return new DoctorPortalPagedResultDto<DoctorPortalWorkDto>
        {
            Page = filter.Page,

            PageSize = filter.PageSize,

            TotalCount = result.TotalCount,

            Items = result.Items
                .Select(work => new DoctorPortalWorkDto
                {
                    Id = work.Id,

                    PatientName =
                        work.PatientName,

                    WorkDate =
                        work.WorkDate,

                    TotalPrice =
                        work.Items.Sum(
                            x => x.TotalAmount),

                    Items = work.Items
                        .Select(item =>
                            new DoctorPortalWorkItemDto
                            {
                                Id = item.Id,

                                WorkTypeName =
                                    item.WorkType?.Name
                                    ?? "Naməlum",

                                Quantity =
                                    item.ToothCount,

                                UnitPrice =
                                    item.UnitPrice,

                                TotalPrice =
                                    item.TotalAmount
                            })
                        .ToList()
                })
                .ToList()
        };
    }

    // =========================================================
    // PAYMENTS
    // =========================================================

    public async Task<
        DoctorPortalPagedResultDto<DoctorPortalPaymentDto>?
    > GetPaymentsAsync(
        Guid accessToken,
        DoctorPortalPaymentFilterDto filter)
    {
        var doctor = await _doctorRepository
            .GetByAccessTokenAsync(accessToken);

        if (doctor == null)
            return null;

        var result =
            await _paymentRepository
                .GetDoctorPortalPaymentsAsync(
                    doctor.Id,
                    filter);

        return new DoctorPortalPagedResultDto<DoctorPortalPaymentDto>
        {
            Page = filter.Page,

            PageSize = filter.PageSize,

            TotalCount = result.TotalCount,

            Items = result.Items
                .Select(payment =>
                    new DoctorPortalPaymentDto
                    {
                        Id = payment.Id,

                        Amount =
                            payment.Amount,

                        PaymentDate =
                            payment.PaymentDate,

                        Note =
                            payment.Note
                    })
                .ToList()
        };
    }
}