using System;

namespace HealthCare.Shared.DTOs.Patient
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string InsuranceId { get; set; }
    }
}