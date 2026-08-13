namespace DentStarLab.Application.DTOs.Works;

public class WorkItemDto
{
    public int Id { get; set; }
    public int WorkTypeId { get; set; }
    public string WorkTypeName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
