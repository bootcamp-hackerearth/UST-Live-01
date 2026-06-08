using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IUserRepository
        : IRepository<User>
    {
        User GetByEmail(
            string email);

        User GetByUserCode(string userCode);
    }
}