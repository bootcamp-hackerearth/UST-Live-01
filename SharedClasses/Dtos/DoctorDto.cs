
using System;
using System.ComponentModel.DataAnnotations;
using SharedClasses.Enums;

namespace SharedClasses.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public Specialisation Specialisation { get; set; }

        [Required]
        public DateTime PracticeStartDate { get; set; }

        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}