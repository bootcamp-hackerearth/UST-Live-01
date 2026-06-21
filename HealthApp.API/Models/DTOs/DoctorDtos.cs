using System.ComponentModel.DataAnnotations;
using HealthApp.API.Enums;

namespace HealthApp.API.Models.DTOs;

public class DoctorDto
{
    public int DoctorId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public SpecialisationType Specialisation { get; set; }
    public int YearsOfExperience { get; set; }
    public int ConsultationFee { get; set; }
    public bool IsActive { get; set; }
}
public class CreateDoctorDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string TemporaryPassword { get; set; } = string.Empty;

    [Required]
    public SpecialisationType Specialisation { get; set; }

    [Required]
    public DateTime PracticeStartDate { get; set; }

    [Range(0, 100000)]
    public int ConsultationFee { get; set; }
}
public class UpdateDoctorDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public SpecialisationType Specialisation { get; set; }

    [Required]
    public DateTime PracticeStartDate { get; set; }

    [Range(0, 100000)]
    public int ConsultationFee { get; set; }

    public bool IsActive { get; set; }
}
public class DoctorAvailabilityDto 
{ 
    public int DoctorId { get; set; } 
    public DateTime Date { get; set; } 
    public List<string> AvailableSlots { get; set; } = new(); 
}
