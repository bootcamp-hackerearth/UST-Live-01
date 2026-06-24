using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Admin.Dtos.Doctors;

public class UpdateDoctorDto
{
    [Required(ErrorMessage = "Doctor name is required.")]
    [MinLength(2, ErrorMessage = "Doctor name must be at least 2 characters.")]
    [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Doctor name must start with a capital letter and contain only letters and spaces.")]
    public string DoctorName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialisation is required.")]
    [RegularExpression(
        "(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)",
        ErrorMessage = "Select a valid specialisation.")]
    public string Specialisation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Years of experience is required.")]
    public int YearsOfExperience { get; set; }

    [Required(ErrorMessage = "Consultation fee is required.")]
    [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
    public int ConsultationFee { get; set; }

    public bool IsActive { get; set; }
}