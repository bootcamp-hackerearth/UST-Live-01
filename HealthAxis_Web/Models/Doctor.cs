using System.Collections.Generic;

namespace HealthAxis.Api.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; }

        public string Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}