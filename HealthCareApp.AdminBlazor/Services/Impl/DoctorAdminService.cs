using HealthCareApp.AdminBlazor.Dtos.Doctors;
using HealthCareApp.AdminBlazor.Services.Interfaces;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class DoctorAdminService : IDoctorAdminService
    {
        private static readonly List<DoctorDto> Doctors = new()
        {
            new DoctorDto
            {
                DoctorId = 1,
                FullName = "Arun Menon",
                Email = "arun.menon@example.com",
                Specialisation = "General Practitioner",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 2,
                FullName = "Meera Nair",
                Email = "meera.nair@example.com",
                Specialisation = "Cardiologist",
                YearsOfExperience = 15,
                ConsultationFee = 1000,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 3,
                FullName = "Vikram Das",
                Email = "vikram.das@example.com",
                Specialisation = "Dermatologist",
                YearsOfExperience = 8,
                ConsultationFee = 700,
                IsActive = true
            }
        };

        public Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = Doctors
                .OrderBy(d => d.DoctorId)
                .ToList();

            return Task.FromResult(doctors);
        }

        public Task<DoctorDto?> GetDoctorByIdAsync(int doctorId)
        {
            var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            return Task.FromResult(doctor);
        }

        public Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto request)
        {
            int nextId = Doctors.Any()
                ? Doctors.Max(d => d.DoctorId) + 1
                : 1;

            var doctor = new DoctorDto
            {
                DoctorId = nextId,
                FullName = request.FullName,
                Email = request.Email,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = request.IsActive
            };

            Doctors.Add(doctor);

            return Task.FromResult(doctor);
        }

        public Task<DoctorDto?> UpdateDoctorAsync(int doctorId, UpdateDoctorDto request)
        {
            var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor is null)
            {
                return Task.FromResult<DoctorDto?>(null);
            }

            doctor.FullName = request.FullName;
            doctor.Email = request.Email;
            doctor.Specialisation = request.Specialisation;
            doctor.YearsOfExperience = request.YearsOfExperience;
            doctor.ConsultationFee = request.ConsultationFee;
            doctor.IsActive = request.IsActive;

            return Task.FromResult<DoctorDto?>(doctor);
        }

        public Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor is null)
            {
                return Task.FromResult(false);
            }

            Doctors.Remove(doctor);

            return Task.FromResult(true);
        }

        public Task<DoctorDto?> ToggleDoctorStatusAsync(int doctorId)
        {
            var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor is null)
            {
                return Task.FromResult<DoctorDto?>(null);
            }

            doctor.IsActive = !doctor.IsActive;

            return Task.FromResult<DoctorDto?>(doctor);
        }
    }
}