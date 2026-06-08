using HealthCare_Appointment_Portal.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.HealthRecordDtos
{
    public class CreateHealthRecordDto
    {
        [Required]
        public int AppointmentId
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

        [Required]
        public int DoctorId
        {
            get;
            set;
        }

        [Required]
        public DateTime VisitDate
        {
            get;
            set;
        }

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