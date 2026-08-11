namespace DentStarLab.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<Work> Works { get; set; } = new List<Work>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}