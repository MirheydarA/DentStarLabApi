using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Application.DTOs.Auth;

public class LogoutRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}
