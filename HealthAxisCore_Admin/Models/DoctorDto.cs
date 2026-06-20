namespace HealthAxisCore_Admin.Models
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Specialisation { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateDoctorDto
    {
        public string DoctorName { get; set; } = string.Empty;

        public string Specialisation { get; set; } = "Cardiologist";

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class UpdateDoctorDto
    {
        public string DoctorName { get; set; } = string.Empty;

        public string Specialisation { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}