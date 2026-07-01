using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

       // [Required(ErrorMessage = "Patient is required")]
        public int? PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        public int DoctorId { get; set; }

       // [Required(ErrorMessage = "Patient name is required")]
        public string PatientName { get; set; } = string.Empty;

       // [Required(ErrorMessage = "Doctor name is required")]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Appointment date is required")]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        public string TimeSlot { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [RegularExpression("Pending|Confirmed|Completed|Cancelled",
            ErrorMessage = "Status must be Pending, Confirmed, Completed, or Cancelled")]
        public string Status { get; set; } = "Pending";

        [MaxLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters")]
        public string? CancellationReason { get; set; }
    }
}