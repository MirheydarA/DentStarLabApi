using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Application.DTOs.Payments;

public class PaymentDto
{
    public int Id { get; set; }

    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }
}
