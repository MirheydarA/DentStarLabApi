using DentStarLab.Application.DTOs.Auth;
using DentStarLab.Application.Interfaces;
using DentStarLab.Domain.Entities;

namespace DentStarLab.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        IUserRepository userRepository,
        PasswordService passwordService,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto dto)
    {
        var user = await _userRepository
            .GetByEmailAsync(dto.Email);

        if (user == null)
            throw new UnauthorizedAccessException(
                "Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException(
                "User is inactive.");

        var passwordValid = _passwordService.Verify(
            user,
            user.PasswordHash,
            dto.Password);

        if (!passwordValid)
            throw new UnauthorizedAccessException(
                "Invalid email or password.");

        var token = _tokenService.GenerateToken(user);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshTokenValue,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task LogoutAsync(LogoutRequestDto dto)
    {
        var refreshToken = await _refreshTokenRepository
            .GetByTokenAsync(dto.RefreshToken);

        if (refreshToken == null || !refreshToken.IsActive)
            return;

        refreshToken.IsRevoked = true;

        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }

    public async Task ChangePasswordAsync(
        int userId,
        ChangePasswordRequestDto dto)
    {
        var user = await _userRepository
            .GetByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException(
                "User not found.");

        var currentPasswordValid = _passwordService.Verify(
            user,
            user.PasswordHash,
            dto.CurrentPassword);

        if (!currentPasswordValid)
            throw new UnauthorizedAccessException(
                "Current password is incorrect.");

        user.PasswordHash = _passwordService.Hash(
            user,
            dto.NewPassword);

        await _userRepository.UpdateAsync(user);

        await _refreshTokenRepository.RevokeAllForUserAsync(user.Id);
    }
}