using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should only contain alphabets")]
        [MinLength(2)]
        public string DoctorName { get; set; }
        [Required]
        [RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
        public string Specialisation { get; set; }
        [Required]
        public int YearsOfExperience { get; set; }
        [Required]
        [Range(0, 100000, ErrorMessage = "Consultation Fee cannot be negative")]
        [Precision(9,2)]
        public decimal ConsultationFee { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
