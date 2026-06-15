using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Dtos.PatientDtos;

public class CreatePatientDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;
}