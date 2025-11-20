using ElearningPlatform.Core.DTOs;
using ElearningPlatform.Core.Entities;
using ElearningPlatform.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElearningPlatform.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Course>> GetPublishedCoursesAsync(string? search, int page, int pageSize)
    {
        var query = _context.Courses
            .Include(c => c.Instructor) // Eager load for "Author Name"
            .Where(c => c.Status == CourseStatus.Published);

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Basic search logic - case-insensitive search in title and description
            query = query.Where(c => c.Title.Contains(search) || c.Description.Contains(search));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .OrderByDescending(c => c.CreatedDate) // Most recent courses first
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Course>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Course?> GetCourseWithCurriculumAsync(int courseId)
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .Include(c => c.Modules.OrderBy(m => m.Order)) // Sort modules
                .ThenInclude(m => m.Lessons.OrderBy(l => l.Order)) // Sort lessons
            .Where(c => c.Status == CourseStatus.Published) // Only published courses
            .FirstOrDefaultAsync(c => c.CourseId == courseId);
    }
}
