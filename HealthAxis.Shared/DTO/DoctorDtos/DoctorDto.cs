using HealthAxis.Shared.Enums;
using System;

namespace HealthAxis.Shared.DTO.DoctorDtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        public string Email { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}