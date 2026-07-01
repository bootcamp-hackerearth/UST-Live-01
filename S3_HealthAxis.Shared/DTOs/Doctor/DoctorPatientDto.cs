using S3_HealthAxis.Shared.Enums;

namespace S3_HealthAxis.Shared.DTOs.Doctor
{
    public class DoctorPatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? InsuranceId { get; set; }

        public bool IsActive { get; set; }

        public int TotalAppointments { get; set; }

        public DateOnly? LastVisitDate { get; set; }
    }
}