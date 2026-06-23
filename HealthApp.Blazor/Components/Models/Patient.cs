using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Blazor.Components.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string? FullName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Invalid gender specified.")]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(450)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? InsuranceId { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(450)]
        public string? IdentityUserId { get; set; }
    }
}