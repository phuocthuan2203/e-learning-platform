using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Exceptions;
using ElearningPlatform.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        catch  (AccountLockedException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }
}
