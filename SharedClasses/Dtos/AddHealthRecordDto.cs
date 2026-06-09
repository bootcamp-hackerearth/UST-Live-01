using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class AddHealthRecordDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AppointmentId { get; set; }

        [Required]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        [StringLength(500)]
        public string Prescription { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}