namespace HealthAxisAdminLayout.DTOs.HealthRecord
{
        public class HealthRecordResponseDTO
        {
            public int HealthRecordId { get; set; }

            public int PatientId { get; set; }

            public int? DoctorId { get; set; }

            public int AppointmentId { get; set; }

            public DateTime VisitDate { get; set; }

            public string Diagnosis { get; set; } = string.Empty;

            public string Prescription { get; set; } = string.Empty;

            public string? Notes { get; set; }
        }
    }