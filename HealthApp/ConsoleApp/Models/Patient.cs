using System;
namespace HealthApp.ConsoleApp.Models
{
    public enum GenderType { Male, Female, Other };

    public class Patient
    {
        public int PatientId { get; set; }
        public required string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string InsuranceId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Calculate age based on Date of Birth
        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }

        // Return summary of the patient's profile
        public string GetProfileSummary()
        {
            return $"ID: {PatientId} | Name: {FullName} | Age: {GetAge()} | Gender: {Gender} | Email: {Email} | Phone: {PhoneNumber}";
        }
    }
}
