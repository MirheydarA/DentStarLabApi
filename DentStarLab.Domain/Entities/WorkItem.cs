using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Domain.Entities;
public class WorkItem
{
    public int Id { get; set; }
    public int WorkId { get; set; }
    public int WorkTypeId { get; set; }
    public int ToothCount { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public Work Work { get; set; } = null!;
    public WorkType WorkType { get; set; } = null!;
}
