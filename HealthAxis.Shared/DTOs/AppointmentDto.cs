using HealthAxis.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace HealthAxis.Shared.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        [Range(1, int.MaxValue)] 
        public int PatientId { get; set; }
        [Display(Name ="Patient Name")]
        public string PatientName { get; set; }
        [Range(1, int.MaxValue)] 
        public int DoctorId { get; set; }
        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; }
        [Display(Name = "Doctor Specialisation")]
        public SpecialisationEnum? DoctorSpecialisation { get; set; }
        [Required, DataType(DataType.Date)] public DateTime ScheduledDate { get; set; }
        [Display(Name ="Time Slot")]
        [Required, StringLength(20)] public string TimeSlot { get; set; }
        public AppointmentStatusEnum Status { get; set; }
        [StringLength(225)] public string CancellationReason { get; set; }
    }
}
