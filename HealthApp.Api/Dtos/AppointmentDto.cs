using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dtos
{
    public class AppointmentDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Patient name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Patient name cannot exceed 50 characters.")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Doctor name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Doctor name cannot exceed 50 characters.")]
        public string DoctorName { get; set; } = string.Empty;

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Time slot must be at least 3 characters.")]
        [StringLength(20, ErrorMessage = "Time slot cannot exceed 20 characters.")]
        public string TimeSlot { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Pending|Confirmed|Completed|Cancelled)$",
            ErrorMessage = "Invalid appointment status.")]
        public string Status { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Cancellation reason cannot exceed 250 characters.")]
        public string? CancellationReason { get; set; }
    }
}