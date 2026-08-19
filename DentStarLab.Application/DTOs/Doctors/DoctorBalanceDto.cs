namespace DentStarLab.Application.DTOs.Doctors;

public class DoctorBalanceDto
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = null!;

    public decimal TotalWorksAmount { get; set; }
    public decimal TotalPaidAmount { get; set; }

    // Borc = Works - Payments
    public decimal Balance { get; set; }
}