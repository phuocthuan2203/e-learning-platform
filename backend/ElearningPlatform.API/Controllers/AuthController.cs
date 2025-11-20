using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Exceptions;
using ElearningPlatform.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ElearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
        }
        catch (EmailExistsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (InvalidCredentialsException)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }
        catch (AccountLockedException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }

    [HttpGet("profile")]
    [Authorize] // <--- Protects this endpoint
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        // Extract UserId from the "sub" claim in the JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized();
        }

        var profile = await _authService.GetUserProfileAsync(userId);
        return Ok(profile);
    }

    [HttpPut("profile")]
    [Authorize] // <--- Protects this endpoint
    public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized();
        }

        var updatedProfile = await _authService.UpdateUserProfileAsync(userId, dto);
        return Ok(updatedProfile);
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized();
        }

        try
        {
            await _authService.ChangePasswordAsync(userId, dto);
            return Ok(new { message = "Password changed successfully" });
        }
        catch (InvalidCredentialsException)
        {
            return BadRequest(new { message = "Current password is incorrect" });
        }
    }
}
