namespace DentStarLab.Application.DTOs.Dashboard;

public class DoctorRevenueDto
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = null!;
    public int WorkCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class MonthlyRevenueDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class DashboardStatsDto
{
    public decimal TotalRevenueThisMonth { get; set; }
    public int TotalWorksThisMonth { get; set; }

    public decimal TotalRevenueThisYear { get; set; }
    public int TotalWorksThisYear { get; set; }

    public List<DoctorRevenueDto> DoctorRevenueThisMonth { get; set; } = new();

    // Son 12 ay (bu ay daxil)
    public List<MonthlyRevenueDto> MonthlyRevenueTrend { get; set; } = new();
}
