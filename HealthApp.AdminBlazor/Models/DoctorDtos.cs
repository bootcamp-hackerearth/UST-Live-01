using System.ComponentModel.DataAnnotations;

namespace HealthApp.AdminBlazor.Models;

public class DoctorDto
{
    public int DoctorId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialisation { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public int ConsultationFee { get; set; }
    public bool IsActive { get; set; }
    public bool MustChangePassword { get; set; }
}

public class CreateDoctorDto
{
    [Required(ErrorMessage = "Doctor full name is required.")]
    [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Doctor name must start with a capital letter and contain only letters and spaces.")]
    [StringLength(100, ErrorMessage = "Doctor name must not exceed 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temporary password is required.")]
    [MinLength(6, ErrorMessage = "Temporary password must be at least 6 characters.")]
    public string TemporaryPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialisation is required.")]
    public string Specialisation { get; set; } = "GeneralPractitioner";

    [Required(ErrorMessage = "Practice start date is required.")]
    public DateTime PracticeStartDate { get; set; } = DateTime.Today.AddYears(-5);

    [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
    public int ConsultationFee { get; set; }
}

public class UpdateDoctorDto
{
    [Required(ErrorMessage = "Doctor full name is required.")]
    [StringLength(100, ErrorMessage = "Doctor name must not exceed 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialisation is required.")]
    public string Specialisation { get; set; } = "GeneralPractitioner";

    [Required(ErrorMessage = "Practice start date is required.")]
    public DateTime PracticeStartDate { get; set; } = DateTime.Today.AddYears(-5);

    [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
    public int ConsultationFee { get; set; }

    public bool IsActive { get; set; } = true;
}
