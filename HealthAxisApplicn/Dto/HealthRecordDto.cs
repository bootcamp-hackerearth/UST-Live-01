using HealthAxisApplicn.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Dto
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime VisitDate { get; set; }
        [Required]
        public string Diagnosis { get; set; }
        [Required]
        public string Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
