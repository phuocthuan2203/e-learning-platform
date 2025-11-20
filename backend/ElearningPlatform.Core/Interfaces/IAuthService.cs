using ElearningPlatform.Core.DTOs;

namespace ElearningPlatform.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    Task<UserProfileDto> GetUserProfileAsync(int userId);
    Task<UserProfileDto> UpdateUserProfileAsync(int userId, UpdateProfileDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}