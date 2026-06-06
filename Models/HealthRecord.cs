using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMvcApp.Models
{
    public class HealthRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Health Record ID")]
        public int HealthRecordId { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid Patient ID is required.")]
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid Doctor ID is required.")]
        [Display(Name = "Doctor ID")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid Appointment ID is required.")]
        [Display(Name = "Appointment ID")]
        public int AppointmentId { get; set; }
        
        public virtual Patient Patient { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Appointment Appointment { get; set; }


        [Required(ErrorMessage = "Visit date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Visit Date")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters.")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required.")]
        [StringLength(500, ErrorMessage = "Prescription cannot exceed 500 characters.")]
        public string Prescription { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string Notes { get; set; }

        public HealthRecord GetSummary()
        {
            return this;
        }
    }
}