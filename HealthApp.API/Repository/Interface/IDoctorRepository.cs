using HealthApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Interface
{
    public interface IDoctorRepository
    {
        Task AddAsync(Doctor doctor);

        Task<List<Doctor>> GetAllAsync();

        Task<Doctor> GetByIdAsync(int id);

        Task UpdateDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(int id, Doctor doctor);
    }
}