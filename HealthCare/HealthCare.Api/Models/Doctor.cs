using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    [Index(nameof(Specialisation),Name ="IX_Doctor_Specialisation")]
    public class Doctor
    {
       
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Specialisation { get; set; }= null!;

        [Required]
        [Range(0,60)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<AvailableSlots> AvailableSlots { get; set; } = [];
        public ICollection<DoctorLeaves> DoctorLeaves { get; set; } = [];

        public ICollection<HealthRecord> HealthRecords { get; set; } = [];

    }
}
