using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Models
{
    public class HealthRecord
    {
        [Key]
        public int HealthRecordId { get; set; }
        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;
        public int AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; } = null!;

        public DateTime VisitDate { get; set; }
        public string? Diagnosis { get; set; } = string.Empty;
        public string? Prescription { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
