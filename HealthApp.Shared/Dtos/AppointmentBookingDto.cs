using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dtos
{
    public class AppointmentBookingDto
    {
        [Required(ErrorMessage = "DoctorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a valid positive number.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required.")]
        public DateTime? ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        [MinLength(3, ErrorMessage = "Time slot must be at least 3 characters.")]
        [StringLength(30, ErrorMessage = "Time slot cannot exceed 30 characters.")]
        public string TimeSlot { get; set; } = string.Empty;
    }
}