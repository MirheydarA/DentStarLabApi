namespace DentStarLab.Application.DTOs.Doctors;
public class DoctorUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Surname { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
