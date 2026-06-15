using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        [MaxLength(512)]
        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }
    }
}
