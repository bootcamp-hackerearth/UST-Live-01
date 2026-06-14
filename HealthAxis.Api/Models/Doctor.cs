using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        [Required]
        [MinLength(2)]
        [RegularExpression(@"[A-Z][A-Za-z\s]")]
        public required string DoctorName { get; set; }
        [Required]
        [RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)", ErrorMessage = "Invalid Specialisation")]
        public required string Specialisation { get; set; }
        [Required]
        public int YearsOfExperience { get; set; }
        [Required]
        [Range(0,100000)]
        public int ConsultationFee { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
