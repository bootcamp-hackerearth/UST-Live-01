using System;
using System.ComponentModel.DataAnnotations;

public class Patient
{
    public int PatientId { get; set; }

    [Required]
    [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should only contin alphabets")]
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    [RegularExpression("[9876][0-9]{9}")]
    public string PhoneNumber { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string InsuranceId { get; set; }
    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; } = true;
}