namespace DentStarLab.Application.DTOs.DoctorPortal;

public class DoctorPortalWorkFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? WorkTypeId { get; set; }
}
