namespace ElearningPlatform.Core.DTOs;

public class CourseSummaryDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
