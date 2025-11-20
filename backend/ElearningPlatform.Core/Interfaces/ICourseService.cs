using ElearningPlatform.Core.DTOs;

namespace ElearningPlatform.Core.Interfaces;

public interface ICourseService
{
    Task<PagedResult<CourseSummaryDto>> GetCatalogAsync(string? search, int page, int pageSize);
    Task<CourseDetailDto> GetCourseDetailsAsync(int courseId);
}
