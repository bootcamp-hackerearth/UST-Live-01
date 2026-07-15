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
        public required string DoctorName { get; set; }

        [Required, RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
        public required string Specialisation { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required, Range(0, 100000)]
        public int ConsultationFee { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, Phone]
        public required string PhoneNumber { get; set; }

        [Required, MinLength(8)]
        public required string Password { get; set; }
    }

    public class UpdateDoctorDto
    {
        [Required, MinLength(2), RegularExpression(@"[A-Z][A-Za-z\s]+")]
        public required string DoctorName { get; set; }

        [Required, RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
        public required string Specialisation { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required, Range(0, 100000)]
        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}