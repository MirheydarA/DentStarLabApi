using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Domain.Entities;
public class Payment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Doctor Doctor { get; set; } = null!;
}