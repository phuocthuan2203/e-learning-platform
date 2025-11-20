using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Entities;

namespace ElearningPlatform.Core.Interfaces;

public interface ICourseRepository
{
    Task<PagedResult<Course>> GetPublishedCoursesAsync(string? search, int page, int pageSize);
    Task<Course?> GetCourseWithCurriculumAsync(int courseId);
}
