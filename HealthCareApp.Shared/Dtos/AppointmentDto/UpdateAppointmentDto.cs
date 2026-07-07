using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Shared.Dtos.Appointments
{
    public class UpdateAppointmentDto

    {
        public string? PatientName { get; set; }

        public string? DoctorName { get; set; }

        [Required]
        public string? AppointmentId { get; set; }
        [Required(ErrorMessage = "Please select the patient for this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid patient reference.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select the doctor for this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid doctor reference.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please enter the updated appointment date.")]
        public DateTime ScheduledDate { get; set; }


        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = string.Empty;

    }
}