using HealthAxisCore_Admin.Models;

namespace HealthAxisCore_Admin.Services
{
    public class DoctorAdminService
    {
        private static readonly List<DoctorDto> Doctors = new()
        {
            new DoctorDto
            {
                DoctorId = 1,
                DoctorName = "Arvind Sharma",
                Specialisation = "Endocrinologist",
                YearsOfExperience = 12,
                ConsultationFee = 700,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 2,
                DoctorName = "Neha Kapoor",
                Specialisation = "Oncologist",
                YearsOfExperience = 15,
                ConsultationFee = 1200,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 3,
                DoctorName = "Priya Raman",
                Specialisation = "Gynecologist",
                YearsOfExperience = 10,
                ConsultationFee = 800,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 4,
                DoctorName = "Vikram Nair",
                Specialisation = "OrthopedicSurgeon",
                YearsOfExperience = 14,
                ConsultationFee = 1000,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 5,
                DoctorName = "Rekha Menon",
                Specialisation = "Psychiatrist",
                YearsOfExperience = 9,
                ConsultationFee = 900,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 6,
                DoctorName = "Suresh Iyer",
                Specialisation = "Pediatrician",
                YearsOfExperience = 11,
                ConsultationFee = 650,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 7,
                DoctorName = "Anita Rao",
                Specialisation = "Neurologist",
                YearsOfExperience = 13,
                ConsultationFee = 1100,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 8,
                DoctorName = "Rohit Das",
                Specialisation = "Dermatologist",
                YearsOfExperience = 8,
                ConsultationFee = 600,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 9,
                DoctorName = "Lakshmi Iyer",
                Specialisation = "Cardiologist",
                YearsOfExperience = 16,
                ConsultationFee = 1300,
                IsActive = true
            },
            new DoctorDto
            {
                DoctorId = 10,
                DoctorName = "Manoj Pillai",
                Specialisation = "GeneralPractitioner",
                YearsOfExperience = 7,
                ConsultationFee = 500,
                IsActive = true
            }
        };

        public Task<List<DoctorDto>> GetDoctorsAsync()
        {
            /*
             * ============================================================
             * TEMPORARY DISCONNECTED VERSION
             * ============================================================
             * Returns hardcoded doctors instead of calling:
             * GET api/admin/doctors
             * ============================================================
             */

            return Task.FromResult(
                Doctors
                    .OrderBy(doctor => doctor.DoctorName)
                    .ToList());
        }

        public Task<DoctorDto?> CreateDoctorAsync(CreateDoctorDto request)
        {
            /*
             * ============================================================
             * TEMPORARY DISCONNECTED VERSION
             * ============================================================
             * Adds doctor to in-memory list instead of calling:
             * POST api/admin/doctors
             * ============================================================
             */

            var nextId = Doctors.Any()
                ? Doctors.Max(doctor => doctor.DoctorId) + 1
                : 1;

            var doctor = new DoctorDto
            {
                DoctorId = nextId,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            Doctors.Add(doctor);

            return Task.FromResult<DoctorDto?>(doctor);
        }

        public Task<DoctorDto?> UpdateDoctorAsync(
            int doctorId,
            UpdateDoctorDto request)
        {
            /*
             * ============================================================
             * TEMPORARY DISCONNECTED VERSION
             * ============================================================
             * Updates doctor in memory instead of calling:
             * PUT api/admin/doctors/{id}
             * ============================================================
             */

            var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor == null)
            {
                return Task.FromResult<DoctorDto?>(null);
            }

            doctor.DoctorName = request.DoctorName;
            doctor.Specialisation = request.Specialisation;
            doctor.YearsOfExperience = request.YearsOfExperience;
            doctor.ConsultationFee = request.ConsultationFee;
            doctor.IsActive = request.IsActive;

            return Task.FromResult<DoctorDto?>(doctor);
        }
    }
}