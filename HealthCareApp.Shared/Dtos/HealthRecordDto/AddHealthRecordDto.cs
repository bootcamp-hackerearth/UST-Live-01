using System.ComponentModel.DataAnnotations;


namespace HealthCareApp.Shared.Dtos.HealthRecords
{
    public class AddHealthRecordDto

    {


        [Required]
        public int PatientId { get; set; }


        public int? DoctorId { get; set; }

        [Required(ErrorMessage = "Please select the appointment linked to this health record.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid appointment reference.")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Please enter the diagnosis details.")]
        [StringLength(500, ErrorMessage = "Diagnosis details must not exceed 500 characters.")]
        public required string Diagnosis { get; set; }

        [Required(ErrorMessage = "Please enter the prescribed treatment or medication.")]
        [StringLength(500, ErrorMessage = "Prescription details must not exceed 500 characters.")]
        public required string Prescription { get; set; }

        [StringLength(1000, ErrorMessage = "Additional notes must not exceed 1000 characters.")]
        public required string Notes { get; set; }


        [Required]
        public DateTime VisitDate { get; set; }

    }
}