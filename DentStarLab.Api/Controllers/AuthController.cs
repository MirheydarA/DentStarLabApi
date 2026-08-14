using System.Security.Claims;
using DentStarLab.Application.DTOs.Auth;
using DentStarLab.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentStarLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutRequestDto dto)
    {
        await _authService.LogoutAsync(dto);

        return Ok(new
        {
            message = "Uğurla çıxış edildi."
        });
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequestDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        try
        {
            await _authService.ChangePasswordAsync(userId, dto);

            return Ok(new
            {
                message = "Şifrə uğurla dəyişdirildi."
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }
}