using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name cannot contain numbers")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required")]
        public string Specialisation { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required")]
        [Range(1, 10000, ErrorMessage = "Enter valid consultation fee")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}
