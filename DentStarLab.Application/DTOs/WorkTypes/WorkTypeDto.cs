namespace DentStarLab.Application.DTOs.WorkTypes;
public class WorkTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PricePerTooth { get; set; }
    public bool IsActive { get; set; }
}