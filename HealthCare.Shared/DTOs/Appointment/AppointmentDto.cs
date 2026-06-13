using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Shared.DTOs.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter a valid Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor selection is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Please select a time slot")]
        public string TimeSlot { get; set; }

        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }
}