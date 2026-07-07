using HealthCareApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCareApp.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "Name should start with a capital letter and contain only alphabets")]
        [MinLength(2)]
        [MaxLength(100)]
        public required string PatientName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        public string? InsuranceID { get; set; }

        public string? IdentityUserId { get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? IdentityUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
    }
}