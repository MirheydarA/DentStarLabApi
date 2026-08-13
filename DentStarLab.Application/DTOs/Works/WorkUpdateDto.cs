namespace DentStarLab.Application.DTOs.Works;

public class WorkUpdateDto
{
    public int DoctorId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime WorkDate { get; set; }
    public List<WorkItemCreateDto> Items { get; set; } = new();
}
