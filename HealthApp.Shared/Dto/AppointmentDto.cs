using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace HealthApp.Shared.Dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;
        [Required]
        public string DoctorName { get; set; } = string.Empty;


        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string? CancellationReason { get; set; }



    }
}