namespace HealthApp.Api.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Specialisation { get; set; }
        public string DoctorPhoneNo { get; set; }
        public string DoctorEmail { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
    }
}
