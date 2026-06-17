using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisHealth.API.Models
{

    [ExcludeFromCodeCoverage]
    public class Doctor
    {

        #region Properties

        [Key]
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = ValidationMessages.DoctorNameRequired)]
        [StringLength(
            ValidationLimits.DoctorNameLength,
            ErrorMessage = ValidationMessages.InvalidDoctorNameFormat)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = ValidationMessages.InvalidDoctorNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        [Required(ErrorMessage = ValidationMessages.ExperienceRequired)]
        [Range(
            ValidationLimits.MinExperience,
            ValidationLimits.MaxExperience,
            ErrorMessage = ValidationMessages.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = ValidationMessages.ConsultationFeeRequired)]
        [Range(
            typeof(decimal),
            ValidationLimits.MinConsultationFee,
            ValidationLimits.MaxConsultationFee,
            ErrorMessage = ValidationMessages.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
            = DateTime.UtcNow;

        #endregion

        #region Navigation Properties

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        #endregion

        #region Business Methods

        public bool IsAvailable(
            DateTime scheduledDate,
            string timeSlot)
        {

            return !Appointments.Any(a =>
                a.ScheduledDate.Date == scheduledDate.Date &&
                a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public int GetUpcomingAppointmentCount()
        {

            return Appointments.Count(a =>
                a.ScheduledDate.Date >= DateTime.Today &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public void Activate()
        {

            IsActive = true;
        }

        public void Deactivate()
        {

            IsActive = false;
        }

        #endregion
    }
}
