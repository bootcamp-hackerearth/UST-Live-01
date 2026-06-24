namespace HealthAxisApplicn.Dto.Admin
{
    public class DoctorAdminDto
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Specialisation { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public string AvailableFrom { get; set; } = string.Empty;

        public string AvailableTo { get; set; } = string.Empty;
    }
}
