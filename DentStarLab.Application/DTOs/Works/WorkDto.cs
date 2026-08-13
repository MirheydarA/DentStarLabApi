namespace DentStarLab.Application.DTOs.Works;

public class WorkDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime WorkDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WorkItemDto> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}
