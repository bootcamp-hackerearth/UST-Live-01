using HealthAxis.API.Enums;

namespace HealthAxis.API.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public Specialisation Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public int UpcomingAppointmentCount { get; set; }
    }
}