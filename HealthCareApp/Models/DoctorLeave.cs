using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Models
{
    public class DoctorLeave
    {
        public int DoctorLeaveId { get; set; }

        public int DoctorId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(300)]
        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Doctor? Doctor { get; set; }
    }
}