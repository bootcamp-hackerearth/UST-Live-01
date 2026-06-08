using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.Models
{

    public class Doctor
    {

        [Key]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = Constants.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Constants.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Constants.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        [Range(
            ValidationLimits.MinExperience,
            ValidationLimits.MaxExperience,
            ErrorMessage = Constants.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        [Range(
            typeof(decimal),
            ValidationLimits.MinConsultationFee,
            ValidationLimits.MaxConsultationFee,
            ErrorMessage = Constants.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        public bool IsAvailable( DateTime scheduledDate,
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
    }
}