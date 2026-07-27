using HealthAxisApplicn.Dto.HealthRecords;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto.Patients
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNo { get; set; } = string.Empty;

        public string? InsuranceID { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreatePatientDto
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Za-z\s]{2,50}$", ErrorMessage = "Name should contain only alphabets and spaces")]
        public string PatientName { get; set; } = string.Empty;

        [Required]

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(Male|Female|Transgender|Other)$", ErrorMessage = "Invalid gender")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+\d{10,15}$", ErrorMessage = "Enter valid number with country code (e.g. +919876543210)")]
        public string PhoneNo { get; set; } = string.Empty;

        [RegularExpression(@"^$|^INS-[A-Z]{2}\d{4}$", ErrorMessage = "Insurance ID must be empty or in format INS-AB1234")]
        public string? InsuranceID { get; set; }

    }


    public class UpdatePatientDto
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Za-z\s]{2,50}$", ErrorMessage = "Name should contain only alphabets and spaces")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(Male|Female|Transgender|Other)$", ErrorMessage = "Invalid gender")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+\d{10,15}$", ErrorMessage = "Enter valid number with country code (e.g. +919876543210)")]
        public string PhoneNo { get; set; } = string.Empty;


        [RegularExpression(@"^$|^INS-[A-Z]{2}\d{4}$", ErrorMessage = "Insurance ID must be empty or in format INS-AB1234")] 
        public string? InsuranceID { get; set; }    

    }


    public class PatientDetailsDto
    {
        public PatientDto Patient { get; set; } = null!;

        public List<HealthRecordDto> HealthRecords { get; set; } = [];
    }


}
