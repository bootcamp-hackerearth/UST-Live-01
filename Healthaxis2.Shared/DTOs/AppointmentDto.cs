using System;

namespace Healthaxis2.Shared.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string Slot { get; set; }

        public string Status { get; set; }

        public string CancellationReason { get; set; }

        // ✅ IMPORTANT (for joins & Mapping)
        public string PatientName { get; set; }

        public string DoctorName { get; set; }
    }
}