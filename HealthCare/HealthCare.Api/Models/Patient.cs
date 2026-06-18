using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class Patient
    {
        public string? UserId { get; set; }
        public int PatientId { get; set; }

        public string FullName { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? InsuranceId { get; set; }
        public bool IsActive { get; set; } = true;

        public User? User { get; set; }
        [ForeignKey(nameof(UserId))]
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<HealthRecord> HealthRecords { get; set; } = [];
    }
}
