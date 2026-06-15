using HealthApp.Api.Model;
using HospitalManagementAPI.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Model
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool? IsActive { get; set; }

    }
}