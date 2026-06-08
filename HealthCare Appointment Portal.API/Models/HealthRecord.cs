using HealthCare_Appointment_Portal.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare_Appointment_Portal.Models
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
        }

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
        }

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
        }

        [Required(
            ErrorMessage =
            Constants.VisitDateRequired)]
        public DateTime VisitDate
        {
            get;
            set;
        }

        [Required(
            ErrorMessage =
            Constants.DiagnosisRequired)]
        [StringLength(
            ValidationLimits.DiagnosisLength)]
        public string Diagnosis
        {
            get;
            set;
        } = string.Empty;

        [Required(
            ErrorMessage =
            Constants.PrescriptionRequired)]
        [StringLength(
            ValidationLimits.PrescriptionLength)]
        public string Prescription
        {
            get;
            set;
        } = string.Empty;

        [StringLength(
            ValidationLimits.NotesLength)]
        public string Notes
        {
            get;
            set;
        } = string.Empty;
    }
}