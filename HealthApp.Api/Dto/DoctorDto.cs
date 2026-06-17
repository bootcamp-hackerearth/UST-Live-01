using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthApp.Api.Dto
{
    public class DoctorDto
    {

        public int DoctorId { get; set; }

        [Required]
        [Range(3, 200)]
        public string? FullName { get; set; }

        [Required]
        public string? Specialisation { get; set; }
        [Required]
        [Range(0, 100 ,ErrorMessage = "Invalid experience specified." )]
        public int YearsOfExperience { get; set; }
        [Required]
        [Range(0, 10000000 ,ErrorMessage = "Invalid consultation fee specified.")]
        public decimal ConsultationFee { get; set; }

        public bool? IsActive { get; set; }
    }
}
