using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Interfaces;

namespace ElearningPlatform.Infrastructure.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<PagedResult<CourseSummaryDto>> GetCatalogAsync(string? search, int page, int pageSize)
    {
        // Validate pagination parameters
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Limit max page size

        var result = await _courseRepository.GetPublishedCoursesAsync(search, page, pageSize);

        // Map Course entities to CourseSummaryDto
        var dtos = result.Items.Select(c => new CourseSummaryDto
        {
            CourseId = c.CourseId,
            Title = c.Title,
            Description = c.Description,
            ImageUrl = c.ImageUrl,
            InstructorName = c.Instructor.Name,
            CreatedDate = c.CreatedDate
        }).ToList();

        return new PagedResult<CourseSummaryDto>
        {
            Items = dtos,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<CourseDetailDto> GetCourseDetailsAsync(int courseId)
    {
        var course = await _courseRepository.GetCourseWithCurriculumAsync(courseId);

        if (course == null)
        {
            throw new KeyNotFoundException($"Course with ID {courseId} not found or not publicly available.");
        }

        // Map Course entity to CourseDetailDto with nested hierarchy
        return new CourseDetailDto
        {
            CourseId = course.CourseId,
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            InstructorName = course.Instructor.Name,
            CreatedDate = course.CreatedDate,
            Modules = course.Modules
                .OrderBy(m => m.Order)
                .Select(m => new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    Title = m.Title,
                    Order = m.Order,
                    Lessons = m.Lessons
                        .OrderBy(l => l.Order)
                        .Select(l => new LessonDto
                        {
                            LessonId = l.LessonId,
                            Title = l.Title,
                            TextContent = l.TextContent,
                            ExternalVideoUrl = l.ExternalVideoUrl,
                            Order = l.Order
                        })
                        .ToList()
                })
                .ToList()
        };
    }
}
