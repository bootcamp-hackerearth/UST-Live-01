using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.DTOs;

public class CreateDoctorLeaveDto
{
    [Required(ErrorMessage = "Leave start date is required.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Leave end date is required.")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Leave reason is required.")]
    [StringLength(
        500,
        MinimumLength = 3,
        ErrorMessage = "Leave reason must be between 3 and 500 characters.")]
    public string Reason { get; set; } = string.Empty;

    public bool ConfirmAppointmentCancellation { get; set; }
}

public class DoctorLeaveDto
{
    public int DoctorLeaveId { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class DoctorLeaveStatusDto
{
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public bool IsOnLeave { get; set; }
    public string Message { get; set; } = string.Empty;
    public DoctorLeaveDto? Leave { get; set; }
}

public class DoctorLeaveImpactDto
{
    public int AffectedAppointmentCount { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class DoctorLeaveCreationResultDto
{
    public DoctorLeaveDto Leave { get; set; } = new();
    public int CancelledAppointmentCount { get; set; }
    public string Message { get; set; } = string.Empty;
}