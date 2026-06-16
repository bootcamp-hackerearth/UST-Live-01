using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.Shared.DTOs.HealthRecordDtos
{

    public class UpdateHealthRecordDto
    {

        #region Properties

        [Required(
            ErrorMessage = ValidationMessages.DiagnosisRequired)]
        [StringLength(
            ValidationLimits.DiagnosisLength)]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(
            ErrorMessage = ValidationMessages.PrescriptionRequired)]
        [StringLength(
            ValidationLimits.PrescriptionLength)]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(
            ValidationLimits.NotesLength)]
        public string Notes { get; set; } = string.Empty;

        #endregion
    }
}
