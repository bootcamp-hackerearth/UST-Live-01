using HealthAxis.Shared.Enums;
using System;

namespace HealthAxis.Shared.DTOs.Patient
{
    public class PatientResponseDto
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public GenderType Gender { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}