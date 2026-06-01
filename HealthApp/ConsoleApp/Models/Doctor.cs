using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthApp.ConsoleApp.Models
{
    // Represents a doctor in the healthcare system
    public class Doctor
    {

        public int DoctorId { get; set; }
        public required string FullName { get; set; }
        public required string Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public List<string> AvailableSlots { get; set; } = new List<string>();

        public List<DateTime> AvailableDates { get; set; } = new List<DateTime>();

        // Check if doctor is available based on leaves, and number of confirmed appointments
        public virtual bool IsAvailable(DateTime date)
        {
            return IsActive && AvailableDates.Any(d => d.Date == date.Date);
        }

        // Count of upcoming confirmed appointments for this doctor
        public string GetScheduleSummary(List<Appointment> appointments)
        {
            int count = appointments.Count(a =>
                a.Doctor.DoctorId == this.DoctorId &&
                a.ScheduledDate.Date >= DateTime.Today &&
                a.Status == AppointmentStatus.Confirmed
            );

            if (count == 0)
            {
                return $"Dr. {FullName} has no upcoming confirmed appointments.";
            }

            return $"Dr. {FullName} has {count} upcoming confirmed appointments.";
        }

        // Formatted string of doctor details
        public override string ToString()
        {
            return $"Doctor ID: {DoctorId} \nFull Name: {FullName} \nSpecialisation: {Specialisation} \nExperience: {YearsOfExperience} years \nConsultation Fee: Rs. {ConsultationFee} \nActive Status: {(IsActive ? "Yes" : "No")}";
        }
    }
}
