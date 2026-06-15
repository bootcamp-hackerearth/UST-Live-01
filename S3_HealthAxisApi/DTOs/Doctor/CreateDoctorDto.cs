namespace S3_HealthAxisApi.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public int Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
    }

}
