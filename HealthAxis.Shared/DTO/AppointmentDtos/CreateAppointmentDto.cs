using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "Patient id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid patient id is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid doctor.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        [StringLength(40, ErrorMessage = "Time slot cannot be more than 40 characters.")]
        public string TimeSlot { get; set; } = string.Empty;
    }
}