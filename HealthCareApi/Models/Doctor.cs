using HealthCareApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace HealthCareApi.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Specialisation { get; set; }

        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
        public virtual ICollection<DoctorAvailableSlot> AvailableSlots { get; set; }
        public virtual ICollection<DoctorLeave> Leaves { get; set; }
    }
}