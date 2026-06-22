using HealthCareApp.AdminBlazor.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Doctors
{
    public class UpdateDoctorDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public SpecialisationType Specialisation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PracticeStartDate { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}