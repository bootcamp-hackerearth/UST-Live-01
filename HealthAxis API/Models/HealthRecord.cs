using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxis.API.Models
{
    public class HealthRecord
    {
        [Key]
        public int RecordId
        {
            get;
            set;
        }

        [Required]
        public int AppointmentId
        {
            get;
            set;
        }

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment
        {
            get;
            set;
        } = null!;

        [Required]
        public int PatientId
        {
            get;
            set;
        }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient
        {
            get;
            set;
        } = null!;

        [Required]
        public int DoctorId
        {
            get;
            set;
        }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor
        {
            get;
            set;
        } = null!;

        [Required(ErrorMessage = Helpers.VisitDateRequired)]
        public DateTime VisitDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = Helpers.DiagnosisRequired)]
        [StringLength(ValidationLimits.DiagnosisLength)]
        public string Diagnosis
        {
            get;
            set;
        } = string.Empty;

        [Required(ErrorMessage = Helpers.PrescriptionRequired)]
        [StringLength(ValidationLimits.PrescriptionLength)]
        public string Prescription
        {
            get;
            set;
        } = string.Empty;

        [StringLength(ValidationLimits.NotesLength)]
        public string Notes
        {
            get;
            set;
        } = string.Empty;
    }
}
