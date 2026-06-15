using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using HealthAxisCore_Api.Enums;
namespace HealthAxisCore_Api.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required byte[] PasswordSalt { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public int? ReferenceId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
