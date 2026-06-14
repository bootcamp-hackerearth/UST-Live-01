using HealthAxisHealth.API.Enums;

namespace HealthAxisHealth.API.DTOs.PatientDtos
{

    public class PatientDto
    {

        #region Properties

        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        #endregion
    }
}
