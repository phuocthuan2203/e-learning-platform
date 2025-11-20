namespace ElearningPlatform.Core.Entities;

public class Course
{
    public int CourseId { get; set; }
    public int InstructorId { get; set; } // FK to Instructor
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.Draft; // Enum
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedDate { get; set; }

    // Navigation Props
    public Instructor Instructor { get; set; } = null!;
    public ICollection<Module> Modules { get; set; } = new List<Module>();
}
