using System.ComponentModel.DataAnnotations;
using HealthApp.Shared.Enums;

namespace HealthApp.Shared.DTOs;

public class PatientDto
{
    public int PatientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public GenderType Gender { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? InsuranceId { get; set; }
    public DateTime CreatedDate { get; set; }
}
public class CreatePatientDto
{
    [Required, StringLength(100)]
    [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Full name must start with an uppercase letter and be at least 3 characters long.")]
    public string FullName { get; set; } = string.Empty;
    [Required] 
    public DateTime DateOfBirth { get; set; }
    [Required] 
    public GenderType Gender { get; set; }
    [Required, RegularExpression(@"[6-9]\d{9}", ErrorMessage = "Invalid phone number.")] 
    public string PhoneNumber { get; set; } = string.Empty;
    [Required, EmailAddress(ErrorMessage = "Invalid email address.")] 
    public string Email { get; set; } = string.Empty;
    public string InsuranceId { get; set; } = string.Empty;
}
public class UpdatePatientDto : CreatePatientDto { }
