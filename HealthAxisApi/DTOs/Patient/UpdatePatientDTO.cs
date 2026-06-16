using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.DTOs.Patient
{
    public class UpdatePatientDTO
    {
        public string PatientName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public GenderType Gender { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? InsuranceID { get; set; }
    }
}
