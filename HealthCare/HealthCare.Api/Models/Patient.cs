using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.Models
{
    public class Patient
    {

        public int PatientId { get; set; }

        public int UsertId { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [RegularExpression("Male|Female|Other")]
        public string Gender { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^[6789]\d{9}$",ErrorMessage="PhoneNumber must start with 6,7,8,9 and be only 10 digit long")]
        public int PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string InsuranceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActived { get; set; }

        public  ICollection<Appointment> Appointments { get; set; }
        public  ICollection<HealthRecord> HealthRecords { get; set; }
    }
}
