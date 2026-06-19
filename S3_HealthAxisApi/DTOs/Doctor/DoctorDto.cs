namespace S3_HealthAxisApi.DTOs.Doctor
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}