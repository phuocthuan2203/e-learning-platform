using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElearningPlatform.API.Controllers;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    [AllowAnonymous] // Critical for UC-2 - Public access
    public async Task<ActionResult<PagedResult<CourseSummaryDto>>> GetCatalog(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _courseService.GetCatalogAsync(search, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Critical for UC-2 - Public access
    public async Task<ActionResult<CourseDetailDto>> GetDetails(int id)
    {
        try
        {
            var result = await _courseService.GetCourseDetailsAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
