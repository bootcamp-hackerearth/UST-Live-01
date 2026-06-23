using HealthApp.Blazor.Components.Models;

namespace HealthApp.Blazor.Components.Services
{
    public class PatientService
    {
        private readonly List<Patient> _patients = new()
        {
            new Patient
            {
                PatientId = 1,
                FullName = "Arun S",
                DateOfBirth = new DateTime(1995, 4, 15),
                Gender = "Male",
                PhoneNumber = "9998887771",
                Email = "arun@example.com",
                InsuranceId = "INS1001",
                CreatedDate = DateTime.Today.AddMonths(-6)
            },
            new Patient
            {
                PatientId = 2,
                FullName = "Divya R",
                DateOfBirth = new DateTime(1998, 8, 20),
                Gender = "Female",
                PhoneNumber = "9998887772",
                Email = "divya@example.com",
                InsuranceId = "INS1002",
                CreatedDate = DateTime.Today.AddMonths(-4)
            },
            new Patient
            {
                PatientId = 3,
                FullName = "Kiran P",
                DateOfBirth = new DateTime(1989, 1, 10),
                Gender = "Male",
                PhoneNumber = "9998887773",
                Email = "kiran@example.com",
                InsuranceId = "INS1003",
                CreatedDate = DateTime.Today.AddMonths(-2)
            }
        };

        public Task<List<Patient>> GetAllPatientsAsync()
        {
            return Task.FromResult(_patients.ToList());
        }

        public Task<int> GetPatientCountAsync()
        {
            return Task.FromResult(_patients.Count);
        }

        public Task<Patient?> GetPatientByIdAsync(int id)
        {
            var patient = _patients.FirstOrDefault(p => p.PatientId == id);
            return Task.FromResult(patient);
        }

        public Task<Patient?> GetMyProfileAsync()
        {
            var patient = _patients.FirstOrDefault(p => p.PatientId == 1);
            return Task.FromResult(patient);
        }

        public Task<Patient?> UpdatePatientAsync(int id, Patient dto)
        {
            var existingPatient = _patients.FirstOrDefault(p => p.PatientId == id);

            if (existingPatient == null)
            {
                return Task.FromResult<Patient?>(null);
            }

            existingPatient.FullName = dto.FullName;
            existingPatient.DateOfBirth = dto.DateOfBirth;
            existingPatient.Gender = dto.Gender;
            existingPatient.PhoneNumber = dto.PhoneNumber;
            existingPatient.Email = dto.Email;
            existingPatient.InsuranceId = dto.InsuranceId;

            return Task.FromResult<Patient?>(existingPatient);
        }
    }
}
