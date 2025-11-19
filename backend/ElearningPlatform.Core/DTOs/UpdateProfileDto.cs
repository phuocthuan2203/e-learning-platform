using System.ComponentModel.DataAnnotations;

namespace ElearningPlatform.Core.DTOs;

public class UpdateProfileDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Bio { get; set; }
}