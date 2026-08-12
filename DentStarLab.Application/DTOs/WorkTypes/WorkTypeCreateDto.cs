using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Application.DTOs.WorkTypes;
public class WorkTypeCreateDto
{
    public string Name { get; set; } = null!;
    public decimal PricePerTooth { get; set; }
}