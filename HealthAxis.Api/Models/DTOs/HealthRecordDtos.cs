using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }
        public int? PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class CreateHealthRecordDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Prescription { get; set; }

        public string? Notes { get; set; }
    }
}