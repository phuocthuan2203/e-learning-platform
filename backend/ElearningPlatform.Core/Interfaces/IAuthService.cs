using ElearningPlatform.Core.DTOs;

namespace ElearningPlatform.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
}