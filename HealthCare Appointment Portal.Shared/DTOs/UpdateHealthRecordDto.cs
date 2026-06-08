using HealthCare_Appointment_Portal.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.HealthRecordDtos
{
    public class UpdateHealthRecordDto
    {
        [Required]
        [StringLength(
            ValidationLimits.DiagnosisLength)]
        public string Diagnosis
        {
            get;
            set;
        }

        [Required]
        [StringLength(
            ValidationLimits.PrescriptionLength)]
        public string Prescription
        {
            get;
            set;
        }

        [StringLength(
            ValidationLimits.NotesLength)]
        public string Notes
        {
            get;
            set;
        }
    }
}