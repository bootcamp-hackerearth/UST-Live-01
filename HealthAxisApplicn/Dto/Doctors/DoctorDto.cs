using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto.Doctors
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should only contain alphabets")]
        [MinLength(2)]
        public string DoctorName { get; set; } = string.Empty;
        [Required]
        [RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
        public string Specialisation { get; set; } = string.Empty;
        [Required]
        public int YearsOfExperience { get; set; }
        [Required]
        [Range(0, 100000, ErrorMessage = "Consultation Fee cannot be negative")]
        public decimal ConsultationFee { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }

    public class CreateDoctorDto
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Z][A-Za-z\s]+$")]
        public string DoctorName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)",
            ErrorMessage = "Invalid Specialisation")]
        public string Specialisation { get; set; } = string.Empty;

        [Required]
        [Range(0, 70)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "Consultation Fee cannot be negative")]
        public decimal ConsultationFee { get; set; }
    }

    public class UpdateDoctorDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        [MinLength(2)]
        public string DoctorName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)",
            ErrorMessage = "Invalid Specialisation")]
        public string Specialisation { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        [Range(0, 100000, ErrorMessage = "Consultation Fee cannot be negative")]
        public decimal ConsultationFee { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
