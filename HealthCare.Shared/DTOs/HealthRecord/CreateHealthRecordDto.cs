using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Shared.DTOs.HealthRecord
{
    public class CreateHealthRecordDto
    {
        public int RecordId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Visit date is required")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Diagnosis must be between 5 and 500 characters")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Prescription must be between 5 and 500 characters")]
        public string Prescription { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string Notes { get; set; }
    }
}