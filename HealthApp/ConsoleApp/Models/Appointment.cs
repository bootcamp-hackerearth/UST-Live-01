using System;
using System.Numerics;
using System.Text;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Models
{
    // Represents a medical appointment between a patient and a doctor
    public class Appointment
    {
        //  Properties
        public int AppointmentId { get; set; }
        public required Patient Patient { get; set; }
        public required Doctor Doctor { get; set; }
        public DateTime ScheduledDate { get; set; }
        public required string TimeSlot { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string CancellationReason { get; set; } = "";


        public Appointment()
        {
            Status = AppointmentStatus.Pending;
        }

        //Confirm the appointment if it's not cancelled
        public void Confirm()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot confirm a cancelled appointment.");
            }

            Status = AppointmentStatus.Confirmed;
        }

        // Cancel the appointment with a reason, but only if it's not already completed
        public void Cancel(string reason)
        {
            if (Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel a completed appointment.");
            }

            Status = AppointmentStatus.Cancelled;
            CancellationReason = reason;
        }

        // Mark the appointment as completed, but only if it's currently confirmed
        public void Complete()
        {
            if (Status != AppointmentStatus.Confirmed)
            {
                throw new InvalidOperationException("Only confirmed appointments can be completed.");
            }

            Status = AppointmentStatus.Completed;
        }

        // Get a detailed string representation of the appointment, including patient and doctor info
        public string GetDetails()
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine($"Appointment ID: {AppointmentId}");
            details.AppendLine($"Patient: {Patient?.Name}");
            details.AppendLine($"Doctor: {Doctor?.Name} ({Doctor?.Specialisation})");
            details.AppendLine($"Date: {ScheduledDate.ToShortDateString()}");
            details.AppendLine($"Time Slot: {TimeSlot}");
            details.AppendLine($"Status: {Status}");

            if (!string.IsNullOrEmpty(CancellationReason))
            {
                details.AppendLine($"Cancellation Reason: {CancellationReason}");
            }

            return details.ToString();
        }
    }
}