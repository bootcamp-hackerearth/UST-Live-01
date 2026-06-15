using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class DoctorLeaves
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }

        [Required]
        public DateOnly LeaveDate { get; set; }

        [MaxLength(300)]
        public string Reason { get; set; }

        public DateTimeOffset CreateDate { get; set; }= DateTimeOffset.UtcNow;

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;
    }
}
