using System;
using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class UpdateAppointmentDto
    {
        [Required(ErrorMessage = "Please select the patient for this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid patient reference.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select the doctor for this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid doctor reference.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please enter the updated appointment date.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Please select an appointment slot.")]
        [Range(1, 16, ErrorMessage = "Appointment slot must be between 1 and 16.")]
        public int SlotNumber { get; set; }
    }
}