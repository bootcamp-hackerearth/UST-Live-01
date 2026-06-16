using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HealthAxis.API.Models
{

    public class Doctor
    {

        [Key]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = Helpers.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Helpers.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        [Range(
            ValidationLimits.MinExperience,
            ValidationLimits.MaxExperience,
            ErrorMessage = Helpers.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        [Range(
            typeof(decimal),
            ValidationLimits.MinConsultationFee,
            ValidationLimits.MaxConsultationFee,
            ErrorMessage = Helpers.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        public bool IsAvailable(DateTime scheduledDate,
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