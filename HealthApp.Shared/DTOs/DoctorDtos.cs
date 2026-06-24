using System.ComponentModel.DataAnnotations;
using HealthApp.Shared.Enums;

namespace HealthApp.Shared.DTOs;

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
    [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Full name must start with Capital letter and contain only letters")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(SpecialisationType), ErrorMessage = "Invalid specialisation.")]
    public SpecialisationType Specialisation { get; set; }

    [Required]
    public DateTime PracticeStartDate { get; set; }

    [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 to 100000.")]
    public int ConsultationFee { get; set; }
}
public class UpdateDoctorDto
{
    [Required, StringLength(100)]
    [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Full name must start with Capital letter and contain only letters")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(SpecialisationType), ErrorMessage = "Invalid specialisation.")]
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

public class CreateDoctorResponseDto
{
    public string Message { get; set; } = string.Empty;
    public DoctorDto Doctor { get; set; } = new();
    public string TemporaryPassword { get; set; } = string.Empty;
}

