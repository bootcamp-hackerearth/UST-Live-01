namespace HealthAxisCore_Admin.Dtos.Doctors;

public class AdminDoctorDto
{
    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public string Specialisation { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public int ConsultationFee { get; set; }

    public bool IsActive { get; set; }
}