using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.API.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [ForeignKey("PatId")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("DocId")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression("(09:00 AM|10:00 AM|11:00 AM|12:00 PM|02:00 PM|03:00 PM|04:00 PM|05:00 PM)")]
        public string TimeSlots { get; set; }

        [Required]
        [RegularExpression("(Pending|Confirmed|Cancelled|Completed)")]
        public string Status { get; set; }

        [Required]
        public string CancellationReason { get; set; }
    }
}
