using ElearningPlatform.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public TestController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }
}