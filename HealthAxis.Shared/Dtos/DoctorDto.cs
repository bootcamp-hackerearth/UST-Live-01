using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{ 
public class DoctorDto
{
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Only alphabets allowed")]
    public string FullName { get; set; }

    [Required]
    public string Specialisation { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Experience cannot be negative")]
    public int YearsOfExperience { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fee cannot be negative")]
    public decimal ConsultationFee { get; set; }

    public bool IsActive { get; set; }
    public int UpcomingAppointments { get; set; }
    }

public class CreateDoctorDto
    {
        public string FullName { get; set; }

        public string Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }
    }

    public class UpdateDoctorDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }

        public string Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}
