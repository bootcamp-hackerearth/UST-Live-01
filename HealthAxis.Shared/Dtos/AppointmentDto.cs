using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string CancellationReason { get; set; }

        public bool CanConfirm { get; set; }
        public bool CanComplete { get; set; }
        public bool CanCancel { get; set; }
        public bool CanAddHealthRecord { get; set; }
    }
}
