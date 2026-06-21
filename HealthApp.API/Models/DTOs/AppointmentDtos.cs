using System.ComponentModel.DataAnnotations;
using HealthApp.API.Enums;

namespace HealthApp.API.Models.DTOs;

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public string? CancellationReason { get; set; }
}
public class BookAppointmentDto
{
    [Range(1,int.MaxValue)] public int PatientId { get; set; }
    [Range(1,int.MaxValue)] public int DoctorId { get; set; }
    [Required] public DateTime ScheduledDate { get; set; }
    [Required, MaxLength(50)] public string TimeSlot { get; set; } = string.Empty;
}
public class UpdateAppointmentStatusDto
{
    [Required] public AppointmentStatus Status { get; set; }
    [MaxLength(200)] public string? CancellationReason { get; set; }
}
public class CancelAppointmentDto 
{ 
    [Range(1,int.MaxValue)] 
    public int AppointmentId { get; set; } 
    [Required, StringLength(200)] 
    public string Reason { get; set; } = string.Empty; 
}
