namespace HealthCare.Api.DTOs.Doctor
{
    public class DoctorFilter : PaginationParam
    {
        public string Specialisation { get; set; }
        public int? MinExperience { get; set; }
    }
}
