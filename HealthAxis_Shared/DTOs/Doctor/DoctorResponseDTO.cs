using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.DTOs.Doctor
{
    public class DoctorResponseDTO
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public SpecialisationType Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
        public string Email { get; set; }
    }
}