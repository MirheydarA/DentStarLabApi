using DentStarLab.Application.DTOs.Payments;
using DentStarLab.Domain.Entities;
using DentStarLab.Application.DTOs.DoctorPortal;
namespace DentStarLab.Application.Interfaces;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
    Task<Payment?> GetByIdAsync(int id);
    Task<(List<Payment> Items, int TotalCount, decimal TotalAmount)> GetPagedAsync(PaymentQueryDto query);    Task UpdateAsync(Payment payment);
    Task DeleteAsync(Payment payment);
    Task SaveChangesAsync();
    Task<List<Payment>> GetByDoctorIdAsync(int doctorId);
    
    Task<(List<Payment> Items,int TotalCount)> GetDoctorPortalPaymentsAsync(int doctorId,DoctorPortalPaymentFilterDto filter);

    Task<decimal> GetDoctorTotalPaymentAmountAsync(int doctorId);

    Task<decimal> GetDoctorCurrentMonthPaymentAmountAsync(int doctorId, DateTime fromDate, DateTime toDate);
    Task<List<(int Year, int Month, decimal Amount)>> GetDoctorPaymentAmountsByMonthAsync(int doctorId);
}