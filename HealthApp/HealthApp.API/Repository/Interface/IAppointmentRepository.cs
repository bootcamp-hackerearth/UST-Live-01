using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace HealthApp.API.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);

        Task<List<Appointment>> GetAllAsync();

        Task<Appointment> GetByIdAsync(int id);

        Task SaveAsync();
    }
}