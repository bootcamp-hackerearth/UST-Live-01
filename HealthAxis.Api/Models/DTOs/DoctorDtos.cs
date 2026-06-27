using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialisation { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public int ConsultationFee { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateDoctorDto
    {
        [Required, MinLength(2), RegularExpression(@"[A-Z][A-Za-z\s]+")]
        public string DoctorName { get; set; }

        [Required, RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
        public string Specialisation { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required, Range(0, 100000)]
        public int ConsultationFee { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }

    public class UpdateDoctorDto
    {
        [Required, MinLength(2), RegularExpression(@"[A-Z][A-Za-z\s]+")]
        public string DoctorName { get; set; }

        [Required, RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
        public string Specialisation { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required, Range(0, 100000)]
        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}