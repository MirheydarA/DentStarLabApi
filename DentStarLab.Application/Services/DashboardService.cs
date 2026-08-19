using DentStarLab.Application.DTOs.Dashboard;
using DentStarLab.Application.Interfaces;


namespace DentStarLab.Application.Services;

public class DashboardService
{
    private readonly IWorkRepository _workRepository;

    public DashboardService(IWorkRepository workRepository)
    {
        _workRepository = workRepository;
    }

    public Task<DashboardStatsDto> GetStatsAsync()
    {
        return _workRepository.GetDashboardStatsAsync();
    }
}
