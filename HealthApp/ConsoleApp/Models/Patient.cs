using System;
namespace HealthApp.ConsoleApp.Models
{
    // Represents a patient in the healthcare system
    public class Patient
    {
        public int PatientId { get; set; }
        public required string Name { get; set; }
        public DateTime Dob { get; set; }
        public GenderType Gender { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string InsuranceId { get; set; } = "";
        public DateTime? CreatedAt { get; set; }

        // Constructor to initialize CreatedAt
        public Patient()
        {
            CreatedAt = DateTime.Now;
        }

        // Method to calculate age based on Dob
        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - Dob.Year;

            if (Dob.Date > today.AddYears(-age))
                age--;

            return age;
        }

        // Method to get a summary of the patient's profile
        public string GetProfileSummary()
        {
            return $"ID: {PatientId} | Name: {Name} | Age: {GetAge()} | Gender: {Gender} | Email: {Email} | Phone: {PhoneNumber}";
        }
    }
}
