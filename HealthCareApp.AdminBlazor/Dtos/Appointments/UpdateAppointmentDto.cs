using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Appointments
{
    public class UpdateAppointmentDto
    {
        [Required(ErrorMessage = "Patient is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid patient.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid doctor.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatusDto Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}