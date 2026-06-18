using HealthAxis.API.Enums;

namespace HealthAxis.API.DTO.PatientDtos
{
    public class PatientDto
    {
        public int PatientId { get; set;  }

        public string? FullName { get; set; }
  
        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

    }
}