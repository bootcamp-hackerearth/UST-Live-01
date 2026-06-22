using HealthCareApp.AdminBlazor.Enums;

namespace HealthCareApp.AdminBlazor.Dtos.Doctors
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public SpecialisationType Specialisation { get; set; }

        public string Email { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}