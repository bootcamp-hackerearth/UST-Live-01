using HealthApp.ConsoleApp.Models;
namespace HealthApp.ConsoleApp.Databases
{
    public class PatientDb
    {
        public List<Patient> Patients = new()
        {
            // Pre-populated patients with unique IDs, contact details, and insurance information
            // PatientId starts from 101 to avoid conflict with test data
            new Patient
            {
                PatientId = 101,
                Name = "Arjun Kumar",
                Dob = new DateTime(1995, 5, 20),
                Gender = GenderType.Male,
                PhoneNumber = "9876543210",
                Email = "arjun@gmail.com",
                InsuranceId = "INS101",
                CreatedAt = DateTime.Now
            },

            new Patient
            {
                PatientId = 102,
                Name = "Kevin Raj",
                Dob = new DateTime(1998, 8, 15),
                Gender = GenderType.Male,
                PhoneNumber = "9123456789",
                Email = "kevin@gmail.com",
                InsuranceId = "INS102",
                CreatedAt = DateTime.Now
            },

            new Patient
            {
                PatientId = 103,
                Name = "Abi Shankar",
                Dob = new DateTime(1992, 3, 10),
                Gender = GenderType.Male,
                PhoneNumber = "9988776655",
                Email = "abi@gmail.com",
                InsuranceId = "INS103",
                CreatedAt = DateTime.Now
            },

            new Patient
            {
                PatientId = 104,
                Name = "Sneha Reddy",
                Dob = new DateTime(2000, 11, 5),
                Gender = GenderType.Female,
                PhoneNumber = "9001122334",
                Email = "sneha@gmail.com",
                InsuranceId = "INS104",
                CreatedAt = DateTime.Now
            },

            new Patient
            {
                PatientId = 105,
                Name = "Rahul Verma",
                Dob = new DateTime(1989, 1, 18),
                Gender = GenderType.Male,
                PhoneNumber = "9556677889",
                Email = "rahul@gmail.com",
                InsuranceId = "INS105",
                CreatedAt = DateTime.Now
            }
        };
    }
}