using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.DTOs.Doctor
{
    public class DoctorResponseDTO
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = null!;

        public SpecialisationType Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}