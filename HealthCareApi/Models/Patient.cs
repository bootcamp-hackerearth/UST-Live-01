using HealthCareApi.Models;
using System;
using System.Collections.Generic;

namespace HealthCareApi.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string InsuranceId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
    }
}