using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Domain.Entities;
public class Work
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime WorkDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Doctor Doctor { get; set; } = null!;
    public ICollection<WorkItem> Items { get; set; } = new List<WorkItem>();
}
