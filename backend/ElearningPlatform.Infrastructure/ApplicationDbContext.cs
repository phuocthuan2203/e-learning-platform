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
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;

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

        // --- COURSE ENTITY CONFIGURATION ---
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(e => e.CourseId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Foreign Key Relationship with Instructor
            entity.HasOne(e => e.Instructor)
                  .WithMany(i => i.Courses)
                  .HasForeignKey(e => e.InstructorId)
                  .OnDelete(DeleteBehavior.Restrict); // Prevent deleting instructor if they have courses
        });

        // --- MODULE ENTITY CONFIGURATION ---
        modelBuilder.Entity<Module>(entity =>
        {
            entity.ToTable("Modules");
            entity.HasKey(e => e.ModuleId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Order).IsRequired();

            // Foreign Key Relationship with Course
            entity.HasOne(e => e.Course)
                  .WithMany(c => c.Modules)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.Cascade); // Delete modules when course is deleted
        });

        // --- LESSON ENTITY CONFIGURATION ---
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("Lessons");
            entity.HasKey(e => e.LessonId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TextContent).HasMaxLength(5000);
            entity.Property(e => e.ExternalVideoUrl).HasMaxLength(500);
            entity.Property(e => e.Order).IsRequired();

            // Foreign Key Relationship with Module
            entity.HasOne(e => e.Module)
                  .WithMany(m => m.Lessons)
                  .HasForeignKey(e => e.ModuleId)
                  .OnDelete(DeleteBehavior.Cascade); // Delete lessons when module is deleted
        });
    }
}