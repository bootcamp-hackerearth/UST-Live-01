namespace S3_HealthAxis.Shared.DTOs.Doctor
{
    public class UpdateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public int Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}
