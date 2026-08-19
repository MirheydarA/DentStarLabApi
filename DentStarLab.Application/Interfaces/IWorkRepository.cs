using DentStarLab.Application.DTOs.Works;
using DentStarLab.Application.DTOs.Dashboard;
using DentStarLab.Domain.Entities;
using DentStarLab.Application.DTOs.DoctorPortal;

namespace DentStarLab.Application.Interfaces;

public interface IWorkRepository
{
    Task AddAsync(Work work);
    Task<List<Work>> GetAllAsync();
    Task<(List<Work> Items, int TotalCount)> GetPagedAsync(WorkQueryDto query);
    Task<Work?> GetByIdAsync(int id);
    Task UpdateAsync(Work work);
    Task DeleteAsync(Work work);
    Task SaveChangesAsync();

    Task<DashboardStatsDto> GetDashboardStatsAsync();
    Task<List<Work>> GetByDoctorIdAsync(int doctorId);
    Task<(
        List<Work> Items,
        int TotalCount
    )> GetDoctorPortalWorksAsync(
        int doctorId,
        DoctorPortalWorkFilterDto filter);

    Task<decimal> GetDoctorTotalWorkAmountAsync(
        int doctorId);

    Task<decimal> GetDoctorCurrentMonthWorkAmountAsync(int doctorId, DateTime fromDate, DateTime toDate);
}