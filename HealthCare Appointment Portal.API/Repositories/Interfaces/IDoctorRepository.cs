using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetByIdAsync(int id);

        Task<IEnumerable<Doctor>> GetAllAsync();

        Task AddAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);

        Task DeleteAsync(int id);

        Task<IEnumerable<Doctor>> GetDoctorsBySpecialisationAsync(
            Specialisation specialisation);
    }
}