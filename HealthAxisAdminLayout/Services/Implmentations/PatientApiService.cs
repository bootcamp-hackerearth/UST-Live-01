using HealthAxisAdminLayout.DTOs.Patient;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class PatientApiService : IPatientApiService
    {
        private readonly List<PatientResponseDTO> _patients = new()
        {
            new PatientResponseDTO
            {
                PatientId = 1,
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = 1,
                Email = "ayushi@test.com",
                PhoneNumber = "9876543210"
            },
            new PatientResponseDTO
            {
                PatientId = 2,
                PatientName = "John",
                DateOfBirth = new DateTime(1998, 5, 10),
                Gender = 0,
                Email = "john@test.com",
                PhoneNumber = "9876543211"
            }
        };

        public Task<List<PatientResponseDTO>> GetPatientsAsync()
        {
            return Task.FromResult(_patients.ToList());
        }

        public Task<PatientResponseDTO?> GetPatientByIdAsync(int id)
        {
            var patient = _patients.FirstOrDefault(p => p.PatientId == id);
            return Task.FromResult(patient);
        }
    }
}