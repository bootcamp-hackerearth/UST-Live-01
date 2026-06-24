using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto

{
    public class HealthRecordDto
    {
        [Required]
        public int RecordId { get; set; }

        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int? DoctorId { get; set; }

        [Required]
        public DateTime? VisitDate { get; set; }

        [Required]
        public string? Diagnosis { get; set; }

        [Required]
        public string? Prescription { get; set; }

        public string? Notes { get; set; }
    }
}