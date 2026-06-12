using System;

namespace HealthAxis.Shared.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;

                if (DateOfBirth > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string InsuranceId { get; set; }

        public bool IsActive { get; set; }

        public int UpcomingAppointments { get; set; }
    }

    public class CreatePatientDto
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string InsuranceId { get; set; }
        public bool IsActive { get; set; }

    }

    public class UpdatePatientDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string InsuranceId { get; set; }
        public bool IsActive { get; set; }

    }
}