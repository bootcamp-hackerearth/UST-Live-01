using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisCore_Api.Models.DTOs
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }
        
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public required int AppointmentId { get; set; }
        public DateTime VisitDate { get; set; }
        public required string Diagnosis { get; set; }
        public required string Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
