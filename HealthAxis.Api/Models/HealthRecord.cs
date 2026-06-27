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

        public Patient Patient { get; set; }

        [ForeignKey("DID")]
        public int? DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        [ForeignKey("AID")]
        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string? Notes { get; set; }
    }
}