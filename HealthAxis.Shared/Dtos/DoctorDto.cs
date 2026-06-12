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
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required")]
        public Specialisation? Specialisation { get; set; }

        [Range(0, 50, ErrorMessage = "Experience cannot be negative")]
        public int? YearsOfExperience { get; set; }

        [Range(1, 10000, ErrorMessage = "Fee must be greater than 0")]
        public decimal? ConsultationFee { get; set; }

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
