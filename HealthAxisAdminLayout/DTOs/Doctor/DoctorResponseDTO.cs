namespace HealthAxisAdminLayout.DTOs.Doctor
{

    public class DoctorResponseDTO
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}
