namespace ElearningPlatform.Core.Entities;

public class Lesson
{
    public int LessonId { get; set; }
    public int ModuleId { get; set; } // FK
    public string Title { get; set; } = string.Empty;
    public string? TextContent { get; set; } // Nullable (video only lesson)
    public string? ExternalVideoUrl { get; set; } // Nullable (text only lesson)
    public int Order { get; set; }

    // Navigation Props
    public Module Module { get; set; } = null!;
}
