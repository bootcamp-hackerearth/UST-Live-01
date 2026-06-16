using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        [ForeignKey("PatientID")]
        public int PatientID { get; set; }
        public required Patient Patient { get; set; }
        [ForeignKey("DoctorID")]
        public int DoctorID { get; set; }
        public required Doctor Doctor { get; set; }
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
