using System.ComponentModel.DataAnnotations;
using HealthApp.API.Enums;

namespace HealthApp.API.Models.DTOs;

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
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required] public DateTime DateOfBirth { get; set; }
    [Required] public GenderType Gender { get; set; }
    [Required, RegularExpression(@"^[0-9]{10}$")] public string PhoneNumber { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, StringLength(30)] public string InsuranceId { get; set; } = string.Empty;
}
public class UpdatePatientDto : CreatePatientDto { }
