namespace ElearningPlatform.Core.Entities;

public class Module
{
    public int ModuleId { get; set; }
    public int CourseId { get; set; } // FK
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; } // For sorting logic

    // Navigation Props
    public Course Course { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
