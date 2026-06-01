using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthApp.ConsoleApp.Models
{
    // Represents a doctor in the healthcare system
    public class Doctor
    {

        public int DoctorId { get; set; }
        public string Name { get; set; } = "";
        public string Specialisation { get; set; } = "";
        public int YearsOfExperience { get; set; } = 0;
        public decimal ConsultationFee { get; set; } = 0;
        public bool IsActive { get; set; }
        public List<string> AvailableSlots { get; set; } = new List<string>();

        public List<DateTime> AvailableDates { get; set; } = new List<DateTime>();

        // Check if doctor is available based on leaves
        public virtual bool IsAvailable(DateTime date)
        {
            return IsActive && AvailableDates.Any(d => d.Date == date.Date);
        }

        // Count of upcoming confirmed appointments for this doctor
        public string GetScheduleSummary(List<Appointment> appointments)
        {
            int count = appointments.Count(a =>
                a.Doctor.DoctorId == DoctorId &&
                a.ScheduledDate.Date >= DateTime.Today &&
                a.Status == AppointmentStatus.Confirmed
            );

            if (count == 0)
            {
                return $"Dr. {Name} has no upcoming confirmed appointments.";
            }

            return $"Dr. {Name} has {count} upcoming confirmed appointments.";
        }
        public override string ToString()
        {
            return $"Doctor ID: {DoctorId} \nFull Name: {Name} \nSpecialisation: {Specialisation} \nExperience: {YearsOfExperience} years \nConsultation Fee: Rs. {ConsultationFee} \nActive Status: {(IsActive ? "Yes" : "No")}";
        }
    }
}
