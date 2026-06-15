using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Dtos.PatientDtos;

public class UpdatePatientDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;
}