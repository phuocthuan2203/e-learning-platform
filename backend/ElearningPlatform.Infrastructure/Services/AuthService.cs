using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Entities;
using ElearningPlatform.Core.Exceptions;
using ElearningPlatform.Core.Interfaces;

using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using ElearningPlatform.Core;
using ElearningPlatform.Infrastructure.Repositories;

namespace ElearningPlatform.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtOptions)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtOptions.Value;
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
        }
        else if (dto.Role == "Instructor")
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
            Token = GenerateJwtToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) throw new InvalidCredentialsException();

        // 1. check account lockout
        if (user.IsLocked)
        {
            if (user.LockoutEnd > DateTime.UtcNow)
            {
                throw new AccountLockedException("Account is locked. Try again later.");
            }

            // unlock if time passed
            user.IsLocked = false;
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await _userRepository.UpdateAsync(user);
        }

        // 2. verify password
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 5)
            {
                user.IsLocked = true;
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                await _userRepository.UpdateAsync(user);
                throw new AccountLockedException("Account locked due to too many failed attempts.");
            }
            await _userRepository.UpdateAsync(user);
            throw new InvalidCredentialsException();
        }

        // 3. success: reset counters
        user.FailedLoginAttempts = 0;
        user.IsLocked = false;
        user.LockoutEnd = null;
        await _userRepository.UpdateAsync(user);

        // 4. determine role
        string role = user is Student ? "Student" : "Instructor";

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Role = role,
            Token = GenerateJwtToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
        };

    }

    private string GenerateJwtToken(User user)
    {
        string role = user is Student ? "Student" : "Instructor";

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, role),
            new Claim("name", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    public async Task<UserProfileDto> GetUserProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found");

        return MapToDto(user);
    }

    public async Task<UserProfileDto> UpdateUserProfileAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found");

        // Update fields
        user.Name = dto.Name;
        user.Bio = dto.Bio;
        user.LastModifiedDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    // Helper method to avoid code duplication
    private UserProfileDto MapToDto(User user)
    {
        return new UserProfileDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Bio = user.Bio,
            Role = user is Student ? "Student" : "Instructor"
        };
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found");

        // 1. Verify Current Password
        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            throw new InvalidCredentialsException(); // Reuse existing exception
        }

        // 2. Hash New Password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        
        // 3. Update Audit Trail
        user.LastModifiedDate = DateTime.UtcNow;

        // 4. Save
        await _userRepository.UpdateAsync(user);
    }
}
