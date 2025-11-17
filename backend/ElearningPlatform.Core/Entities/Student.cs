namespace ElearningPlatform.Core.Entities;

public class Student : User
{
    // This will have the same value as UserId for the one-to-one mapping
    public int StudentId { get; set; }

    // Navigation properties for future use cases will go here
    // public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}