using HealthApp.Api.Enums;

namespace HealthApp.Api.Dtos
{
    public class DoctorCreateDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public SpecialisationType Specialisation { get; set; }
        public string DoctorPhoneNo { get; set; }
        public string DoctorEmail { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
    }

}
