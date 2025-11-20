namespace ElearningPlatform.Core.DTOs;

public class ModuleDto
{
    public int ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public List<LessonDto> Lessons { get; set; } = new();
}
