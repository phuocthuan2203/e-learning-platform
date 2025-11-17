using Microsoft.EntityFrameworkCore;
using ElearningPlatform.Core.Entities;

namespace ElearningPlatform.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Add DbSet properties for each entity
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Instructor> Instructors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- USER ENTITY CONFIGURATION ---
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique(); // Enforce unique email
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.FailedLoginAttempts).HasDefaultValue(0);
        });

        // --- STUDENT TPT CONFIGURATION ---
        modelBuilder.Entity<Student>().ToTable("Students");

        // --- INSTRUCTOR TPT CONFIGURATION ---
        modelBuilder.Entity<Instructor>().ToTable("Instructors");
    }
}