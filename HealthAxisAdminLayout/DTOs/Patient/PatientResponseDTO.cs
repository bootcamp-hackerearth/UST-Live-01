namespace HealthAxisAdminLayout.DTOs.Patient
{

    public class PatientResponseDTO
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
