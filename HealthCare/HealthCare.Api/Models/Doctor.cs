using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation
       // [ForeignKey("UserId")]
      //  public virtual User User { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
       // public virtual ICollection<DoctorAvailableSlot> AvailableSlots { get; set; }
       // public virtual ICollection<DoctorLeave> Leaves { get; set; }

    }
}
