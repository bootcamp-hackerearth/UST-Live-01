using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>>
            GetAllAsync();

        Task<User>
            GetByIdAsync(
                int userId);

        Task
            AddAsync(
                User user);

        Task
            UpdateAsync(
                User user);

        Task
            DeleteAsync(
                int userId);

        User GetByEmail(
            string email);

        User GetByUserCode(
            string userCode);
    }
}