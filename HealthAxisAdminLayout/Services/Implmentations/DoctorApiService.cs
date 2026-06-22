using HealthAxisAdminLayout.DTOs.Doctor;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly List<DoctorResponseDTO> _doctors = new()
        {
            new DoctorResponseDTO
            {
                DoctorId = 1,
                DoctorName = "Rahul Sharma",
                Email = "rahul.doctor@test.com",
                Specialisation = 0,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            },
            new DoctorResponseDTO
            {
                DoctorId = 2,
                DoctorName = "Mily Srivastava",
                Email = "mily@example.com",
                Specialisation = 1,
                YearsOfExperience = 6,
                ConsultationFee = 1000,
                IsActive = true
            }
        };

        public Task<List<DoctorResponseDTO>> GetDoctorsAsync()
        {
            return Task.FromResult(_doctors.ToList());
        }

        public Task<DoctorResponseDTO?> GetDoctorByIdAsync(int id)
        {
            var doctor = _doctors.FirstOrDefault(d => d.DoctorId == id);
            return Task.FromResult(doctor);
        }

        public Task<bool> CreateDoctorAsync(CreateDoctorDTO dto)
        {
            var newDoctor = new DoctorResponseDTO
            {
                DoctorId = _doctors.Any() ? _doctors.Max(d => d.DoctorId) + 1 : 1,
                DoctorName = dto.DoctorName,
                Email = dto.Email,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            _doctors.Add(newDoctor);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = _doctors.FirstOrDefault(d => d.DoctorId == id);

            if (doctor == null)
                return Task.FromResult(false);

            _doctors.Remove(doctor);
            return Task.FromResult(true);
        }

        public Task<bool> SetDoctorStatusAsync(int doctorId, bool status)
        {
            var doctor = _doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor == null)
                return Task.FromResult(false);

            doctor.IsActive = status;
            return Task.FromResult(true);
        }
    }
}