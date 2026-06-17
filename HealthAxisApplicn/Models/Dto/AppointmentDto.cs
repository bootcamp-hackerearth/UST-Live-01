using HealthAxisApplicn.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Models.Dto
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }
        [Required]
        public string TimeSlot { get; set; }
        [RegularExpression("(Pending|Confirmed|Completed|CAncelled)")]
        public string Status { get; set; }
        [MaxLength(200)]
        public string CancellationReason { get; set; }
    }
}
