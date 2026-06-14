using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Name must start with a capital letter and contain only letters")]
        public string DoctorName { get; set; }

        [Required]
        [RegularExpression("(GeneralPhysician|Cardiologist|Dermatologist|Neurologist|Orthopedic|Pediatrician|Psychiatrist|ENT|Gynecologist)", ErrorMessage = "Invalid specialisation")]
        public string Specialisation { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000")]
        public int ConsultationFee { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
    }
}
