namespace ElearningPlatform.Core.Entities;

public abstract class User
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Bio { get; set; }

    // Security fields from the state machine
    public bool IsLocked { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockoutEnd { get; set; }

    // Audit fields
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}