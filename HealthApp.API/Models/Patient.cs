using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models;

public class Patient
{
    public int PatientId { get; set; }
    public string? UserId { get; set; }

    [Required, RegularExpression(@"[A-Z][a-zA-Z\s]{2,}")]
    public string PatientName { get; set; } = string.Empty;
    [Required] 
    public DateTime DateOfBirth { get; set; }
    [Required, RegularExpression("(Male|Female|Transgender|Other)")]
    public string Gender { get; set; } = string.Empty;
    [EmailAddress] 
    public string? Email { get; set; }
    [Required, RegularExpression(@"[6-9]\d{9}")]
    public string PhoneNumber { get; set; } = string.Empty;
    public string? InsuranceId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public ICollection<Appointment>? Appointments { get; set; }
    public ICollection<HealthRecord>? HealthRecords { get; set; }
}
