using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs.HealthRecord
{
    public class CreateHealthRecordDTO
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment ID is required")]
        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required")]
        public string Prescription { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
