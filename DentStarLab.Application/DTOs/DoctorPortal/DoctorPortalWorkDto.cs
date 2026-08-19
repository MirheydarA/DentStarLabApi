using DentStarLab.Application.DTOs.DoctorPortal;

namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalWorkDto
{
    public int Id { get; set; }

    public string PatientName { get; set; } = null!;

    public DateTime WorkDate { get; set; }

    public decimal TotalPrice { get; set; }

    public List<DoctorPortalWorkItemDto> Items { get; set; } = new();
}