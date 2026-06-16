namespace HealthCare.Api.DTOs.Doctor
{
    public class DoctorListDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = null!;
        public string Specialisation { get; set; } = null!;
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
    }
}
