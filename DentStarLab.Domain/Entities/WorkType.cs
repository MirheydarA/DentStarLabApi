using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Domain.Entities;
public class WorkType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PricePerTooth { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}