namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class AdminPatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}