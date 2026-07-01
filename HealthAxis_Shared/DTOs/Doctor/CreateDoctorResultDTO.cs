using HealthAxis.Shared.Enums;
using System.Text.Json.Serialization;

namespace HealthAxis.Shared.DTOs.Doctor
{
    public class CreateDoctorResultDTO
    {
        public int YearsOfExperience;

        public int DoctorId { get; set; }

        public string? DoctorName { get; set; }

        public string? Email { get; set; }

        public string? TemporaryPassword { get; set; }

        public string? TempPassword { get; set; }

        public string? GeneratedPassword { get; set; }

        public string? Password { get; set; }

        [JsonIgnore]
        public string? DisplayTemporaryPassword =>
            !string.IsNullOrWhiteSpace(TemporaryPassword) ? TemporaryPassword :
            !string.IsNullOrWhiteSpace(TempPassword) ? TempPassword :
            !string.IsNullOrWhiteSpace(GeneratedPassword) ? GeneratedPassword :
            !string.IsNullOrWhiteSpace(Password) ? Password :
            null;

        public SpecialisationType Specialisation { get; set; }
        public int ConsultationFee { get; set; }
        public bool IsActive { get; set; }
    }
}