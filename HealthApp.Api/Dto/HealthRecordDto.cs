using HealthApp.Api.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Dto
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }
        [Required]
        public string? Diagnosis { get; set; }
        [Required]
        public string? Prescription { get; set; }

        public string? Notes { get; set; }

    }
}
