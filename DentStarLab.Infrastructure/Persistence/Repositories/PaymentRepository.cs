using DentStarLab.Application.DTOs.Payments;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DentStarLab.Application.DTOs.DoctorPortal;
namespace DentStarLab.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(List<Payment> Items, int TotalCount, decimal TotalAmount)> GetPagedAsync(PaymentQueryDto query)
    {
        IQueryable<Payment>? paymentsQuery = _context.Payments.Include(p => p.Doctor).AsQueryable();

        if (query.DoctorId.HasValue && query.DoctorId > 0)
        {
            paymentsQuery = paymentsQuery.Where(p => p.DoctorId == query.DoctorId);
        }

        if (query.DateFrom.HasValue)
        {
            paymentsQuery = paymentsQuery.Where(p => p.PaymentDate >= query.DateFrom.Value);
        }

        if (query.DateTo.HasValue)
        {
            paymentsQuery = paymentsQuery.Where(p => p.PaymentDate <= query.DateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();
            paymentsQuery = paymentsQuery.Where(p =>
                p.Note != null && p.Note.ToLower().Contains(search));
        }

        paymentsQuery = paymentsQuery.OrderByDescending(p => p.PaymentDate);

        var totalCount = await paymentsQuery.CountAsync();
        var totalAmount = await paymentsQuery.SumAsync(p => p.Amount);

        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        var items = await paymentsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount, totalAmount);
    }
    
    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Payment payment)
    {
        _context.Payments.Remove(payment);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Payment>> GetByDoctorIdAsync(int doctorId)
    {
        return await _context.Payments
            .Where(x => x.DoctorId == doctorId)
            .OrderByDescending(x => x.PaymentDate)
            .ToListAsync();
    }

    public async Task<(List<Payment> Items, int TotalCount)> GetDoctorPortalPaymentsAsync(int doctorId, DoctorPortalPaymentFilterDto filter)
    {
        var query = _context.Payments
            .AsNoTracking()
            .Where(x => x.DoctorId == doctorId)
            .AsQueryable();

        // =========================================================
        // FROM DATE
        // =========================================================

        if (filter.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.PaymentDate >= filter.FromDate.Value);
        }

        // =========================================================
        // TO DATE
        // =========================================================

        if (filter.ToDate.HasValue)
        {
            var toDateExclusive =
                filter.ToDate.Value.Date.AddDays(1);

            query = query.Where(x =>
                x.PaymentDate < toDateExclusive);
        }

        // =========================================================
        // TOTAL COUNT
        // =========================================================

        var totalCount = await query.CountAsync();

        // =========================================================
        // PAGINATION
        // =========================================================

        var page = filter.Page < 1
            ? 1
            : filter.Page;

        var pageSize = filter.PageSize <= 0
            ? 10
            : Math.Min(filter.PageSize, 50);

        var items = await query
            .OrderByDescending(x => x.PaymentDate)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (
            items,
            totalCount
        );
    }

    public async Task<decimal> GetDoctorTotalPaymentAmountAsync(int doctorId)
    {
        return await _context.Payments
            .Where(x => x.DoctorId == doctorId)
            .SumAsync(x => x.Amount);
    }

    public async Task<decimal> GetDoctorCurrentMonthPaymentAmountAsync(int doctorId, DateTime fromDate, DateTime toDate)
    {
        return await _context.Payments
            .Where(x =>
                x.DoctorId == doctorId &&
                x.PaymentDate >= fromDate &&
                x.PaymentDate < toDate)
            .SumAsync(x => x.Amount);
    }

    public async Task<List<(int Year, int Month, decimal Amount)>> GetDoctorPaymentAmountsByMonthAsync(int doctorId)
    {
        var grouped = await _context.Payments
            .Where(x => x.DoctorId == doctorId)
            .GroupBy(x => new
            {
                x.PaymentDate.Year,
                x.PaymentDate.Month
            })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Amount = g.Sum(x => x.Amount)
            })
            .ToListAsync();

        return grouped.Select(x => (x.Year, x.Month, x.Amount)).ToList();
    }
    
}