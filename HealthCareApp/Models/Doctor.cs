using System.ComponentModel.DataAnnotations;
using HealthCareApp.Shared.Enums;

namespace HealthCareApp.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "Name should start with a capital letter and contain only alphabets")]
        public string DoctorName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public SpecialisationType Specialisation { get; set; }

        [Required]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "Fee must be between 0 and 100000")]
        public int ConsultationFee { get; set; }

        public string? IdentityUserId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public bool MustChangePassword { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Appointment>? Appointments { get; set; }

        public ICollection<HealthRecord>? HealthRecords { get; set; }
        public ICollection<DoctorLeave> Leaves { get; set; } = new List<DoctorLeave>();
    }
}