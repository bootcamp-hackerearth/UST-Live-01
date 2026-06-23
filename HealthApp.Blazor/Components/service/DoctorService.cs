using HealthApp.Blazor.Components.Models;

namespace HealthApp.Blazor.Components.Services
{
    public class DoctorService
    {
        private readonly List<Doctor> _doctors = new()
        {
            new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Anil Kumar",
                Specialisation = "Cardiology",
                PracticeStartDate = new DateTime(2018, 6, 10),
                ConsultationFee = 500,
                Email = "anil.kumar@healthapp.com",
                DoctorPhoneNumber = "9876543210",
                IsActive = true
            },
            new Doctor
            {
                DoctorId = 2,
                FullName = "Dr. Meera Nair",
                Specialisation = "Dermatology",
                PracticeStartDate = new DateTime(2020, 2, 15),
                ConsultationFee = 400,
                Email = "meera.nair@healthapp.com",
                DoctorPhoneNumber = "9876501234",
                IsActive = true
            },
            new Doctor
            {
                DoctorId = 3,
                FullName = "Dr. Rahul Menon",
                Specialisation = "Orthopedics",
                PracticeStartDate = new DateTime(2016, 11, 5),
                ConsultationFee = 600,
                Email = "rahul.menon@healthapp.com",
                DoctorPhoneNumber = "9123456780",
                IsActive = false
            }
        };

        public Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return Task.FromResult(_doctors.ToList());
        }

        public Task<int> GetDoctorCountAsync()
        {
            return Task.FromResult(_doctors.Count);
        }

        public Task<List<Doctor>> GetAllActiveDoctorAsync()
        {
            var result = _doctors.Where(d => d.IsActive == true).ToList();
            return Task.FromResult(result);
        }

        public Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            var doctor = _doctors.FirstOrDefault(d => d.DoctorId == id);
            return Task.FromResult(doctor);
        }

        public Task<List<Doctor>> SearchBySpecialisationAsync(string specialisation)
        {
            var result = _doctors
                .Where(d => !string.IsNullOrWhiteSpace(d.Specialisation) &&
                            d.Specialisation.Contains(specialisation, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Task.FromResult(result);
        }
    }
}