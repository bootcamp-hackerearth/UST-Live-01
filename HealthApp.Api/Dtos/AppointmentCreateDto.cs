using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dtos
{
    public class AppointmentCreateDto
    {
        [Required(ErrorMessage = "PatientId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a valid positive number.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a valid positive number.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        [MinLength(3, ErrorMessage = "Time slot must be at least 3 characters.")]
        [StringLength(20, ErrorMessage = "Time slot cannot exceed 20 characters.")]
        public string TimeSlot { get; set; }
    }
}