namespace ElearningPlatform.Core.Entities;

public class Instructor : User
{
    // This will have the same value as UserId for the one-to-one mapping
    public int InstructorId { get; set; }

    // Navigation properties for future use cases will go here
    // public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}