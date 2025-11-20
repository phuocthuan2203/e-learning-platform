namespace ElearningPlatform.Core.DTOs;

public class LessonDto
{
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TextContent { get; set; }
    public string? ExternalVideoUrl { get; set; }
    public int Order { get; set; }
}
