using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Models
{

    [Index(nameof(Email),IsUnique=true)]
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [AllowedValues("Patient","Doctor","Admin",ErrorMessage="Role must be Admin,Patient or Doctor")]
        public string Role { get; set; } = null;

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; } = null;

        [MaxLength(512)]
        public string RefreshToken { get; set; }

        public DateTimeOffset RefreshTokenExpiry { get; set; }= DateTimeOffset.UtcNow;

        public Patient? Patient { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
