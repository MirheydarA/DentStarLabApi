namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalSummaryDto
{
    public string DoctorName { get; set; } = null!;
    public decimal CurrentBalance { get; set; }
    public decimal CurrentMonthWorkAmount { get; set; }
    public decimal CurrentMonthPaymentAmount { get; set; }
    public DateTime CurrentMonthStart { get; set; }
    public DateTime CurrentMonthEnd { get; set; }
    public decimal CurrentMonthBalance { get; set; }
}