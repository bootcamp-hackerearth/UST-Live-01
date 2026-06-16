using HealthAxisHealth.Shared.Enums;

namespace HealthAxisHealth.Shared.DTOs.DoctorDtos
{
    public class DoctorDto
    {
        #region Properties

        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        #endregion
    }
}
