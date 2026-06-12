using System;

namespace Healthaxis2.Shared.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string InsuranceId { get; set; }

        public DateTime RegisteredDate { get; set; }
    }
}