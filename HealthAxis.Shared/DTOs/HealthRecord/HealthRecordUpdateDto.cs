using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace HealthAxis.API.DTOs.HealthRecords
{
    public class HealthRecordUpdateDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = Helpers.VisitDateRequired)]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = Helpers.DiagnosisRequired)]
        [StringLength(ValidationLimits.DiagnosisLength)]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.PrescriptionRequired)]
        [StringLength(ValidationLimits.PrescriptionLength)]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(ValidationLimits.NotesLength)]
        public string Notes { get; set; } = string.Empty;
    }
}
