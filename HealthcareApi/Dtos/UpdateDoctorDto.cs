using HealthcareApi.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class UpdateDoctorDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public Specialisation Specialisation { get; set; }

        [Required]
        public DateTime PracticeStartDate { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}