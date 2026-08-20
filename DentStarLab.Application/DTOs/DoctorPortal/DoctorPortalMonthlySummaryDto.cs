namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalMonthlySummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal WorkAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal MonthlyBalance { get; set; }
    public decimal CumulativeBalance { get; set; }
}