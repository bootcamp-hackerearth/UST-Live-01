using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class AddHealthRecordDto
    {
        [Required(ErrorMessage = "Please select the appointment linked to this health record.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid appointment reference.")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Please enter the diagnosis details.")]
        [StringLength(500, ErrorMessage = "Diagnosis details must not exceed 500 characters.")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Please enter the prescribed treatment or medication.")]
        [StringLength(500, ErrorMessage = "Prescription details must not exceed 500 characters.")]
        public string Prescription { get; set; }

        [StringLength(1000, ErrorMessage = "Additional notes must not exceed 1000 characters.")]
        public string Notes { get; set; }
    }
}