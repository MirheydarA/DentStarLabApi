namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalDto
{
    public string DoctorName { get; set; } = null!;

    public decimal TotalWorkAmount { get; set; }

    public decimal TotalPaymentAmount { get; set; }

    public decimal RemainingDebt { get; set; }

    public List<DoctorPortalWorkDto> Works { get; set; } = new();

    public List<DoctorPortalPaymentDto> Payments { get; set; } = new();
}