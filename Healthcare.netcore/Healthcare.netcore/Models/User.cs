using HealthAxis.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    public bool IsActive { get; set; } = true;

    public Doctor? Doctor { get; set; }

    public Patient? Patient { get; set; }
}