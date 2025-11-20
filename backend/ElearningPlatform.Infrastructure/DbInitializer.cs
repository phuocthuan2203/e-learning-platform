using ElearningPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElearningPlatform.Infrastructure;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Ensure database is created
        context.Database.Migrate();

        // Check if we already have data
        if (context.Courses.Any())
        {
            return; // DB has been seeded
        }

        // Create Instructors (inheriting from User)
        var instructors = new Instructor[]
        {
            new Instructor
            {
                Name = "Dr. Sarah Johnson",
                Email = "sarah.johnson@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Bio = "Expert in Web Development with 10+ years of experience",
                CreatedDate = DateTime.UtcNow,
                IsLocked = false,
                FailedLoginAttempts = 0
            },
            new Instructor
            {
                Name = "Prof. Michael Chen",
                Email = "michael.chen@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Bio = "Data Science and Machine Learning specialist",
                CreatedDate = DateTime.UtcNow,
                IsLocked = false,
                FailedLoginAttempts = 0
            }
        };

        context.Instructors.AddRange(instructors);
        context.SaveChanges();

        // Create Courses with Modules and Lessons
        var courses = new Course[]
        {
            new Course
            {
                InstructorId = instructors[0].UserId,
                Title = "Modern Web Development with Angular",
                Description = "Learn to build modern web applications using Angular framework, TypeScript, and best practices.",
                ImageUrl = "https://example.com/images/angular-course.jpg",
                Status = CourseStatus.Published,
                CreatedDate = DateTime.UtcNow,
                Modules = new List<Module>
                {
                    new Module
                    {
                        Title = "Introduction to Angular",
                        Order = 1,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "What is Angular?",
                                TextContent = "Angular is a platform and framework for building single-page client applications using HTML and TypeScript.",
                                Order = 1
                            },
                            new Lesson
                            {
                                Title = "Setting up your development environment",
                                ExternalVideoUrl = "https://youtube.com/watch?v=example1",
                                TextContent = "In this lesson, we'll set up Node.js, npm, and Angular CLI.",
                                Order = 2
                            }
                        }
                    },
                    new Module
                    {
                        Title = "Components and Templates",
                        Order = 2,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "Creating your first component",
                                ExternalVideoUrl = "https://youtube.com/watch?v=example2",
                                Order = 1
                            },
                            new Lesson
                            {
                                Title = "Component lifecycle hooks",
                                TextContent = "Lifecycle hooks allow you to tap into key moments in the component lifecycle.",
                                Order = 2
                            }
                        }
                    }
                }
            },
            new Course
            {
                InstructorId = instructors[1].UserId,
                Title = "Machine Learning Fundamentals",
                Description = "Master the basics of machine learning including supervised and unsupervised learning algorithms.",
                ImageUrl = "https://example.com/images/ml-course.jpg",
                Status = CourseStatus.Published,
                CreatedDate = DateTime.UtcNow,
                Modules = new List<Module>
                {
                    new Module
                    {
                        Title = "Introduction to Machine Learning",
                        Order = 1,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "What is Machine Learning?",
                                TextContent = "Machine Learning is a subset of AI that enables systems to learn from data.",
                                Order = 1
                            },
                            new Lesson
                            {
                                Title = "Types of Machine Learning",
                                ExternalVideoUrl = "https://youtube.com/watch?v=ml-example1",
                                Order = 2
                            }
                        }
                    }
                }
            },
            new Course
            {
                InstructorId = instructors[0].UserId,
                Title = "Advanced TypeScript Patterns",
                Description = "Deep dive into TypeScript advanced features, design patterns, and best practices.",
                ImageUrl = "https://example.com/images/typescript-course.jpg",
                Status = CourseStatus.Draft,
                CreatedDate = DateTime.UtcNow,
                Modules = new List<Module>
                {
                    new Module
                    {
                        Title = "TypeScript Basics Review",
                        Order = 1,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "Type System Overview",
                                TextContent = "Review of TypeScript's type system and basic types.",
                                Order = 1
                            }
                        }
                    }
                }
            },
            new Course
            {
                InstructorId = instructors[1].UserId,
                Title = "Data Science with Python",
                Description = "Learn data analysis, visualization, and statistical modeling using Python and popular libraries.",
                ImageUrl = "https://example.com/images/python-ds-course.jpg",
                Status = CourseStatus.Published,
                CreatedDate = DateTime.UtcNow,
                Modules = new List<Module>
                {
                    new Module
                    {
                        Title = "Python for Data Science",
                        Order = 1,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "NumPy Basics",
                                TextContent = "Introduction to NumPy arrays and operations.",
                                Order = 1
                            },
                            new Lesson
                            {
                                Title = "Pandas DataFrames",
                                ExternalVideoUrl = "https://youtube.com/watch?v=pandas-example",
                                Order = 2
                            }
                        }
                    },
                    new Module
                    {
                        Title = "Data Visualization",
                        Order = 2,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "Matplotlib Tutorial",
                                ExternalVideoUrl = "https://youtube.com/watch?v=matplotlib-example",
                                Order = 1
                            }
                        }
                    }
                }
            },
            new Course
            {
                InstructorId = instructors[0].UserId,
                Title = "RESTful API Design",
                Description = "Learn how to design and implement RESTful APIs using ASP.NET Core.",
                ImageUrl = "https://example.com/images/api-course.jpg",
                Status = CourseStatus.Published,
                CreatedDate = DateTime.UtcNow,
                Modules = new List<Module>
                {
                    new Module
                    {
                        Title = "REST Fundamentals",
                        Order = 1,
                        Lessons = new List<Lesson>
                        {
                            new Lesson
                            {
                                Title = "What is REST?",
                                TextContent = "REST is an architectural style for distributed hypermedia systems.",
                                Order = 1
                            },
                            new Lesson
                            {
                                Title = "HTTP Methods and Status Codes",
                                TextContent = "Understanding GET, POST, PUT, DELETE and HTTP status codes.",
                                Order = 2
                            },
                            new Lesson
                            {
                                Title = "Building Your First API",
                                ExternalVideoUrl = "https://youtube.com/watch?v=api-example",
                                Order = 3
                            }
                        }
                    }
                }
            }
        };

        context.Courses.AddRange(courses);
        context.SaveChanges();
    }
}
