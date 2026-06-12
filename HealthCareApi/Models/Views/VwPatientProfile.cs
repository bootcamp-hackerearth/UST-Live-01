using System.ComponentModel.DataAnnotations;

namespace HealthCareApi.Models.Views
{
    public class VwPatientProfile
    {
        [Key]
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string InsuranceId { get; set; }
        public int? AppointmentCount { get; set; }
    }
}