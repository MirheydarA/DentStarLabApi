namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalWorkItemDto
{
    public int Id { get; set; }

    public string WorkTypeName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}