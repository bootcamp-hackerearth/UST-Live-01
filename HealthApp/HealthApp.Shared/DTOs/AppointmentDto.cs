using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Patient Id is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor Id is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        public string TimeSlot { get; set; }

        public string Status { get; set; }
        public string CancellationReason { get; set; }


        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}