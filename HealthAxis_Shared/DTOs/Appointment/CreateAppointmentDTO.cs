using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs.Appointment
{
    public class CreateAppointmentDTO
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        public string TimeSlot { get; set; } = string.Empty;
    }
}