using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisHealth.API.Models
{

    [ExcludeFromCodeCoverage]
    public class HealthRecord
    {

        #region Properties

        [Key]
        public int RecordId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.AppointmentRequired)]
        public int AppointmentId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.PatientRequired)]
        public int PatientId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.DoctorRequired)]
        public int DoctorId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.VisitDateRequired)]
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

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

        #region Navigation Properties

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment { get; set; } = null!;

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; } = null!;

        #endregion

        #region Business Methods

        public bool IsRecentRecord()
        {

            return VisitDate.Date >= DateTime.Today.AddDays(-30);
        }

        #endregion
    }
}
