using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Application.DTOs.Payments;

public class PaymentQueryDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? DoctorId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Search { get; set; }
}
