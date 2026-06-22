using S3_HealthAxis.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace S3_HealthAxisApi.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(100, ErrorMessage = "Doctor name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required.")]
        public DoctorSpecialisation Specialisation { get; set; }

        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0.01, 100000, ErrorMessage = "Consultation fee must be greater than zero.")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();
    }
}