using System.ComponentModel.DataAnnotations;
using HealthAxis.Shared.Enums;

namespace HealthAxisCore_Api.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        // ✅ Doctor Name
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "Name should start with a capital letter and contain only alphabets")]
        public string DoctorName { get; set; } = null!;

        // ✅ ✅ NEW: Email (REQUIRED 🔥)
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        // ✅ Specialization
        [Required]
        public SpecialisationType Specialisation { get; set; }

        // ✅ Experience
        [Required]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years")]
        public int YearsOfExperience { get; set; }

        // ✅ Fee
        [Required]
        [Range(0, 100000, ErrorMessage = "Fee must be between 0 and 100000")]
        public int ConsultationFee { get; set; }

        // ✅ Active status
        [Required]
        public bool IsActive { get; set; } = true;

        // ✅ Created date
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}