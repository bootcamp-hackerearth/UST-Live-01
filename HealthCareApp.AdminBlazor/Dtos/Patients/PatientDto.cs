namespace HealthCareApp.AdminBlazor.Dtos.Patients
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? InsuranceId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
