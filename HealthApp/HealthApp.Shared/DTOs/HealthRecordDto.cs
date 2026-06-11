using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.DTOs
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Patient Id is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor Id is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Visit date is required")]
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required")]
        public string Prescription { get; set; }

        public string Notes { get; set; }

        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}