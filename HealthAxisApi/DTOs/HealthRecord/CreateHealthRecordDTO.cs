using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.HealthRecord
{
    public class CreateHealthRecordDTO
    {
        [Required]
        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        [Required]
        public string Prescription { get; set; } = null!;

        public string? Notes { get; set; }
    }
}