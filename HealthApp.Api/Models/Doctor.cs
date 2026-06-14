using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Models
{

    public enum SpecialisationType
    {
        GeneralPhysician,
        Cardiologist,
        Dermatologist,
        Neurologist,
        Orthopedic,
        Pediatrician,
        Psychiatrist,
        ENT,
        Gynecologist
    }

    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [MinLength(3, ErrorMessage = "Doctor name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Doctor name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\.\-]+$", ErrorMessage = "Doctor name can contain only letters, spaces, dot, and hyphen.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Doctor phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Doctor phone number must be exactly 10 digits.")]
        public string DoctorPhoneNo { get; set; }

        [Required(ErrorMessage = "Doctor email is required.")]
        [EmailAddress(ErrorMessage = "Invalid doctor email address.")]
        [StringLength(150, ErrorMessage = "Doctor email cannot exceed 150 characters.")]
        public string DoctorEmail { get; set; }

        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public virtual User User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<HealthRecord> HealthRecords { get; set; }

    }
}
