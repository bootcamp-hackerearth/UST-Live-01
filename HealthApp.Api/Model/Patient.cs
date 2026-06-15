using HospitalManagementAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Model
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string InsuranceId { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}