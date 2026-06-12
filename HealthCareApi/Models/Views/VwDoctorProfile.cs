using System.ComponentModel.DataAnnotations;

namespace HealthCareApi.Models.Views
{
    public class VwDoctorProfile
    {
        [Key]
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public int? TotalAppointments { get; set; }
    }
}