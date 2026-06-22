using HealthCareApp.AdminBlazor.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Patients
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string DateOfBirth { get; set; } = string.Empty;

        public GenderType Gender { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string InsuranceId { get; set; } = string.Empty;

        public string CreatedDate { get; set; } = string.Empty;
    }
}