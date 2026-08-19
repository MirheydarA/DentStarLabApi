namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalPaymentDto
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Note { get; set; }
}