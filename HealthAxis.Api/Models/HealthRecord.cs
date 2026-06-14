using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisCore_Api.Models
{
    public class HealthRecord
    {
        [Key]
        public int HealthRecordId { get; set; }
        [ForeignKey("PID")]
        public int? PatientId { get; set; }
        public required Patient Patient { get; set; }
        [ForeignKey("DID")]
        public int? DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        [ForeignKey("AID")]
        public required int AppointmentId { get; set; }
        public required Appointment Appointment { get; set; }
        public DateTime VisitDate { get; set; }
        public required string Diagnosis { get; set; }
        public required string Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
