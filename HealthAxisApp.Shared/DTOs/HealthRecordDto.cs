using HealthAxisApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisApp.Shared.DTOs
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID is required.")]
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Doctor ID is required.")]
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public SpecialisationEnum? DoctorSpecialisation { get; set; }

        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required.")]
        [StringLength(500)]
        public string Prescription { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
