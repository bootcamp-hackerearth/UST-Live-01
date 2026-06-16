namespace HealthApp.Api.Dtos
{
    public class PatientCreateDto
    {
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? InsuranceId { get; set; }
    }
}
