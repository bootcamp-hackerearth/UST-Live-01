using HealthAppMVC.Enums;
using HealthAppWebAPI.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllAsync();

        Task<Doctor> GetByIdAsync(int id);

        Task AddAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);

        Task ChangeStatusAsync(int id, bool isActive);

        Task<List<Doctor>> GetBySpecialisationAsync(
            SpecialisationType specialisation);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> EmailExistsForOtherDoctorAsync(
            int doctorId,
            string email);
    }
}