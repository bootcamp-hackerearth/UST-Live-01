using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Enums;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(
            int id);

        Task<IEnumerable<User>>
            GetAllAsync();

        Task AddAsync(
            User user);

        Task UpdateAsync(
            User user);

        Task DeleteAsync(
            int id);

        User GetByEmail(
            string email);

        User GetByUserCode(
            string userCode);
        Task<User> GetByReferenceIdAsync(
            int referenceId,
            Role role);
    }
}