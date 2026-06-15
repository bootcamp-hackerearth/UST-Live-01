using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Model
{
    public class HealthRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        public string? Diagnosis { get; set; }

        public string? Prescription { get; set; }

        public string? Notes { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient? Patient { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor? Doctor { get; set; }
    }
}