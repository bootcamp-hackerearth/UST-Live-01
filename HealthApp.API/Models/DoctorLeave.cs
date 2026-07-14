using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models;

public class DoctorLeave
{
    public int DoctorLeaveId { get; set; }

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}