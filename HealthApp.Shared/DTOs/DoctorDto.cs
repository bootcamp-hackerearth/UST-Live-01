using HealthApp.Shared.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Invalid Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}