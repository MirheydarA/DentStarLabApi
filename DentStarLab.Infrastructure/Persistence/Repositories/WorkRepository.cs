using DentStarLab.Application.DTOs.Dashboard;
using DentStarLab.Application.DTOs.DoctorPortal;
using DentStarLab.Application.DTOs.Works;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentStarLab.Infrastructure.Persistence.Repositories;

public class WorkRepository : IWorkRepository
{
    private readonly AppDbContext _context;

    public WorkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Work work)
    {
        await _context.Works.AddAsync(work);
    }

    public async Task<List<Work>> GetAllAsync()
    {
        return await _context.Works
            .Include(x => x.Doctor)
            .Include(x => x.Items)
                .ThenInclude(x => x.WorkType)
            .ToListAsync();
    }

    public async Task<(List<Work> Items, int TotalCount)> GetPagedAsync(
        WorkQueryDto query)
    {
        var worksQuery = _context.Works
            .Include(x => x.Doctor)
            .Include(x => x.Items)
                .ThenInclude(x => x.WorkType)
            .AsQueryable();

        if (query.DoctorId.HasValue && query.DoctorId > 0)
        {
            worksQuery = worksQuery.Where(w => w.DoctorId == query.DoctorId);
        }

        if (query.DateFrom.HasValue)
        {
            worksQuery = worksQuery.Where(w => w.WorkDate >= query.DateFrom.Value);
        }

        if (query.DateTo.HasValue)
        {
            worksQuery = worksQuery.Where(w => w.WorkDate <= query.DateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();

            worksQuery = worksQuery.Where(w =>
                w.PatientName.ToLower().Contains(search));
        }

        worksQuery = worksQuery.OrderByDescending(w => w.WorkDate);

        var totalCount = await worksQuery.CountAsync();

        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        var items = await worksQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Work?> GetByIdAsync(int id)
    {
        return await _context.Works
            .Include(x => x.Doctor)
            .Include(x => x.Items)
                .ThenInclude(x => x.WorkType)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Work work)
    {
        _context.Works.Update(work);

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Work work)
    {
        _context.Works.Remove(work);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // =====================================================
    // DASHBOARD STATS
    // =====================================================

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var today = DateTime.UtcNow.Date;

        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var startOfNextMonth = startOfMonth.AddMonths(1);

        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfNextYear = startOfYear.AddYears(1);

        // Trend üçün son 12 ayın başlanğıcı (bu ay daxil)
        var startOfTrend = startOfMonth.AddMonths(-11);

        // =================================================
        // Bu ay
        // =================================================

        var worksThisMonth = await _context.Works
            .Include(w => w.Doctor)
            .Include(w => w.Items)
            .Where(w => w.WorkDate >= startOfMonth && w.WorkDate < startOfNextMonth)
            .ToListAsync();

        var totalRevenueThisMonth = worksThisMonth
            .SelectMany(w => w.Items)
            .Sum(i => i.TotalAmount);

        var doctorRevenueThisMonth = worksThisMonth
            .GroupBy(w => new
            {
                w.DoctorId,
                DoctorName = $"{w.Doctor.Name} {w.Doctor.Surname}"
            })
            .Select(g => new DoctorRevenueDto
            {
                DoctorId = g.Key.DoctorId,
                DoctorName = g.Key.DoctorName,
                WorkCount = g.Count(),
                TotalRevenue = g.SelectMany(w => w.Items).Sum(i => i.TotalAmount)
            })
            .OrderByDescending(d => d.TotalRevenue)
            .ToList();

        // =================================================
        // Bu il
        // =================================================

        var worksThisYear = await _context.Works
            .Include(w => w.Items)
            .Where(w => w.WorkDate >= startOfYear && w.WorkDate < startOfNextYear)
            .ToListAsync();

        var totalRevenueThisYear = worksThisYear
            .SelectMany(w => w.Items)
            .Sum(i => i.TotalAmount);

        // =================================================
        // Son 12 ay trend
        // =================================================

        var worksForTrend = await _context.Works
            .Include(w => w.Items)
            .Where(w => w.WorkDate >= startOfTrend && w.WorkDate < startOfNextMonth)
            .ToListAsync();

        var monthlyRevenueTrend = new List<MonthlyRevenueDto>();

        for (var i = 0; i < 12; i++)
        {
            var monthStart = startOfTrend.AddMonths(i);
            var monthEnd = monthStart.AddMonths(1);

            var worksInMonth = worksForTrend
                .Where(w => w.WorkDate >= monthStart && w.WorkDate < monthEnd)
                .ToList();

            monthlyRevenueTrend.Add(new MonthlyRevenueDto
            {
                Year = monthStart.Year,
                Month = monthStart.Month,
                WorkCount = worksInMonth.Count,
                TotalRevenue = worksInMonth
                    .SelectMany(w => w.Items)
                    .Sum(i => i.TotalAmount)
            });
        }

        return new DashboardStatsDto
        {
            TotalRevenueThisMonth = totalRevenueThisMonth,
            TotalWorksThisMonth = worksThisMonth.Count,

            TotalRevenueThisYear = totalRevenueThisYear,
            TotalWorksThisYear = worksThisYear.Count,

            DoctorRevenueThisMonth = doctorRevenueThisMonth,
            MonthlyRevenueTrend = monthlyRevenueTrend
        };
    }
    public async Task<List<Work>> GetByDoctorIdAsync(int doctorId)
    {
        return await _context.Works
            .Include(x => x.Items)
            .ThenInclude(x => x.WorkType)
            .Where(x => x.DoctorId == doctorId)
            .OrderByDescending(x => x.WorkDate)
            .ToListAsync();
    }

    public async Task<(List<Work> Items, int TotalCount)> GetDoctorPortalWorksAsync(int doctorId, DoctorPortalWorkFilterDto filter)
    {
        var query = _context.Works
            .AsNoTracking()
            .Where(x => x.DoctorId == doctorId)
            .Include(x => x.Items)
            .ThenInclude(x => x.WorkType)
            .AsQueryable();

        // =========================================================
        // SEARCH
        // =========================================================

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();

            query = query.Where(x =>
                x.PatientName.Contains(search));
        }

        // =========================================================
        // FROM DATE
        // =========================================================

        if (filter.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.WorkDate >= filter.FromDate.Value);
        }

        // =========================================================
        // TO DATE
        // =========================================================

        if (filter.ToDate.HasValue)
        {
            var toDateExclusive =
                filter.ToDate.Value.Date.AddDays(1);

            query = query.Where(x =>
                x.WorkDate < toDateExclusive);
        }

        // =========================================================
        // WORK TYPE
        // =========================================================

        if (filter.WorkTypeId.HasValue)
        {
            query = query.Where(x =>
                x.Items.Any(item =>
                    item.WorkTypeId == filter.WorkTypeId.Value));
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
            .OrderByDescending(x => x.WorkDate)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (
            items,
            totalCount
        );
    }
    public async Task<decimal> GetDoctorTotalWorkAmountAsync(
    int doctorId)
    {
        return await _context.WorkItems
            .Where(x =>
                x.Work.DoctorId == doctorId)
            .SumAsync(x => x.TotalAmount);
    }

    public async Task<decimal> GetDoctorCurrentMonthWorkAmountAsync(int doctorId, DateTime fromDate, DateTime toDate)
    {
        return await _context.WorkItems
            .Where(x =>
                x.Work.DoctorId == doctorId &&
                x.Work.WorkDate >= fromDate &&
                x.Work.WorkDate < toDate)
                .SumAsync(x => x.TotalAmount);
    }

    public async Task<List<(int Year, int Month, decimal Amount)>> GetDoctorWorkAmountsByMonthAsync(int doctorId)
    {
        var raw = await _context.WorkItems
            .Where(x => x.Work.DoctorId == doctorId)
            .Select(x => new
            {
                x.Work.WorkDate,
                x.TotalAmount
            }).ToListAsync();

        return raw
            .GroupBy(x => new { x.WorkDate.Year, x.WorkDate.Month })
            .Select(g => (
                Year: g.Key.Year,
                Month: g.Key.Month,
                Amount: g.Sum(x => x.TotalAmount)
            )).ToList();
    }

    public async Task<List<int>> GetFrequentDoctorIdsAsync(int days, int top)
    {
        var cutoff = DateTime.Now.AddDays(-days);

        var result = await _context.Works
            .Where(w => w.WorkDate >= cutoff)
            .GroupBy(w => w.DoctorId)
            .Select(g => new
            {
                DoctorId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(top)
            .ToListAsync();

        return result.Select(x => x.DoctorId).ToList();
    }
}