using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentStarLab.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;
}
