using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Models
{
    public class HealthRecord
    {
        [Key]
        public int HealthRecordId { get; set; }
        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public required Patient Patient { get; set; }
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        [ForeignKey("AppointmentId")]
        public int AppointmentId { get; set; }
        public required Appointment Appointment { get; set; }
        public DateTime VisitDate { get; set; }
        [Required]
        public string Diagnosis { get; set; }
        [Required]
        public string Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
