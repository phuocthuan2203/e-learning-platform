namespace ElearningPlatform.Core.Entities;

public class Instructor : User
{
    // This will have the same value as UserId for the one-to-one mapping
    public int InstructorId { get; set; }

    // Navigation properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}