using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthaxis2.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        // ✅ FUTURE DATE VALIDATION
        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Appointment), nameof(ValidateAppointmentDate))]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression(@"^(09:00 AM|10:00 AM|11:00 AM|02:00 PM|03:00 PM)$")]
        public string Slot { get; set; }

        [Required]
        [RegularExpression(@"^(Pending|Cancelled|Confirmed|Completed)$")]
        public string Status { get; set; }

        public string CancellationReason { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }

        // ✅ CUSTOM VALIDATION (TOMORROW ONLY)
        public static ValidationResult ValidateAppointmentDate(DateTime date, ValidationContext context)
        {
            if (date <= DateTime.Today)
                return new ValidationResult("Appointments can only be booked from tomorrow onward");

            return ValidationResult.Success;
        }
    }
}