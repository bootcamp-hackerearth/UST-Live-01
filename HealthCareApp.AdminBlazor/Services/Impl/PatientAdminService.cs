using HealthCareApp.AdminBlazor.Dtos.Patients;
using HealthCareApp.AdminBlazor.Services.Interfaces;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class PatientAdminService : IPatientAdminService
    {
        private static readonly List<PatientDto> Patients = new()
        {
            new PatientDto
            {
                PatientId = 1,
                FullName = "Ravi Kumar",
                DateOfBirth = new DateTime(1998, 5, 12),
                Gender = "Male",
                Email = "ravi.kumar@example.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS1001",
                CreatedDate = new DateTime(2026, 6, 15)
            },
            new PatientDto
            {
                PatientId = 2,
                FullName = "Anjali Nair",
                DateOfBirth = new DateTime(2001, 8, 20),
                Gender = "Female",
                Email = "anjali.nair@example.com",
                PhoneNumber = "8765432109",
                InsuranceId = "INS1002",
                CreatedDate = new DateTime(2026, 6, 15)
            },
            new PatientDto
            {
                PatientId = 3,
                FullName = "Kiran Das",
                DateOfBirth = new DateTime(1995, 11, 3),
                Gender = "Other",
                Email = "kiran.das@example.com",
                PhoneNumber = "7654321098",
                InsuranceId = null,
                CreatedDate = new DateTime(2026, 6, 15)
            }
        };

        public Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = Patients
                .OrderBy(p => p.PatientId)
                .ToList();

            return Task.FromResult(patients);
        }

        public Task<PatientDto?> GetPatientByIdAsync(int patientId)
        {
            var patient = Patients.FirstOrDefault(p => p.PatientId == patientId);

            return Task.FromResult(patient);
        }

        public Task<PatientDto> CreatePatientAsync(CreatePatientDto request)
        {
            int nextId = Patients.Any()
                ? Patients.Max(p => p.PatientId) + 1
                : 1;

            var patient = new PatientDto
            {
                PatientId = nextId,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth.Date,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                InsuranceId = request.InsuranceId,
                CreatedDate = DateTime.Today
            };

            Patients.Add(patient);

            return Task.FromResult(patient);
        }

        public Task<PatientDto?> UpdatePatientAsync(int patientId, UpdatePatientDto request)
        {
            var patient = Patients.FirstOrDefault(p => p.PatientId == patientId);

            if (patient is null)
            {
                return Task.FromResult<PatientDto?>(null);
            }

            patient.FullName = request.FullName;
            patient.DateOfBirth = request.DateOfBirth.Date;
            patient.Gender = request.Gender;
            patient.Email = request.Email;
            patient.PhoneNumber = request.PhoneNumber;
            patient.InsuranceId = request.InsuranceId;

            return Task.FromResult<PatientDto?>(patient);
        }

        public Task<bool> DeletePatientAsync(int patientId)
        {
            var patient = Patients.FirstOrDefault(p => p.PatientId == patientId);

            if (patient is null)
            {
                return Task.FromResult(false);
            }

            Patients.Remove(patient);

            return Task.FromResult(true);
        }
    }
}