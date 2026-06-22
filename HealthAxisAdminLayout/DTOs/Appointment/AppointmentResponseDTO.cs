namespace HealthAxisAdminLayout.DTOs.Appointment
{
    
        public class AppointmentResponseDTO
        {
            public int AppointmentId { get; set; }

            public int PatientId { get; set; }

            public int DoctorId { get; set; }

            public DateTime ScheduledDate { get; set; }

            public string TimeSlot { get; set; } = string.Empty;

            public int Status { get; set; }

            public string? CancellationReason { get; set; }
        }
    }