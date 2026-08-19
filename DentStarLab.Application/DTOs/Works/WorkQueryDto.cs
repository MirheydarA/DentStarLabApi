using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Application.DTOs.Works;

public class WorkQueryDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? DoctorId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    // "son ay" kimi hazır filtrlər üçün, istəyə bağlı
    public string? Search { get; set; }       
}
