using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApi.Models.Views
{
    public class VwPatientAppointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialisation { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }
}