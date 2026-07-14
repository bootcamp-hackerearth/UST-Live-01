using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models;

public class Doctor
{
    public int DoctorId { get; set; }
    public string? UserId { get; set; }
    [Required, RegularExpression(@"[A-Z][a-zA-Z\s]{2,}")]
    public string DoctorName { get; set; } = string.Empty;
    [Required, RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)")]
    public string Specialisation { get; set; } = string.Empty;
    [Range(0,50)] public int YearsOfExperience { get; set; }
    [Range(0,100000)] public int ConsultationFee { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public ICollection<Appointment>? Appointments { get; set; }
    public ICollection<HealthRecord>? HealthRecords { get; set; }
    public ICollection<DoctorLeave>? DoctorLeaves { get; set; }
}
