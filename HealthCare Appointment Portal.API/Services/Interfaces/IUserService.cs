using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);

        Task<User> GetByUserCodeAsync(
            string userCode);

        Task<User> GetByEmailAsync(
            string email);
    }
}