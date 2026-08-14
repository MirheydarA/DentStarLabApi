using DentStarLab.Application.DTOs.Auth;
namespace DentStarLab.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task LogoutAsync(LogoutRequestDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordRequestDto dto);
}
