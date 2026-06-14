using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.API.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }

        [ForeignKey("PatId")]
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("DocId")]
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("AppId")]
        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Prescription { get; set; }

        public string? Notes { get; set; }
    }
}
