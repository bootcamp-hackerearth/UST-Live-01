using HealthAxis.API.Enums;

namespace HealthAxis.API.DTO.DoctorDtos
{
    public class UpdateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}