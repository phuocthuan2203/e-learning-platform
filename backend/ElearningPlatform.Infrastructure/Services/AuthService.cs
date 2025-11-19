using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Entities;
using ElearningPlatform.Core.Exceptions;
using ElearningPlatform.Core.Interfaces;

namespace ElearningPlatform.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        // 1. check if email exists
        if (await _userRepository.ExistsAsync(dto.Email))
        {
            throw new EmailExistsException(dto.Email);
        }

        // 2. hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 3. create Entity based on Role
        User user;
        if (dto.Role == "Student")
        {
            user = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                CreatedDate = DateTime.UtcNow
            };
        } else if (dto.Role == "Instructor")
        {
            user = new Instructor
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                CreatedDate = DateTime.UtcNow
            };
        }
        else
        {
            throw new ArgumentException("Invalid role");
        }

        // 4. save to Database
        await _userRepository.AddAsync(user);

        // 5. return response dto
        // NOTE: We are NOT generating a real JWT token yet (Stage 2).
        // We return an empty token for now to satisfy the DTO.
        return new AuthResponseDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Role = dto.Role,
            Token = "TOKEN_GENERATION_IN_STAGE_2",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }
}
