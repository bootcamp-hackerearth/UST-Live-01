using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class CancelledAppointmentArchive
    {
        [Key]
        public int CancelledAppointmentArchiveId { get; set; }

        public int OriginalAppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string CancellationReason { get; set; } = string.Empty;

        public string CancelledByRole { get; set; } = string.Empty;

        public string CancelledByUserId { get; set; } = string.Empty;

        public DateTime CancelledAt { get; set; }

        public bool WasAutoCancelled { get; set; }

        public DateTime ArchivedAt { get; set; }

        public bool LegalHold { get; set; }
    }
}