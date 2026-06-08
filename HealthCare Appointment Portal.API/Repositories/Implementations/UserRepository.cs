using System.Linq;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class UserRepository
        : Repository<User>,
          IUserRepository
    {
        public UserRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public User GetByEmail(
            string email)
        {
            return _dbSet
                .FirstOrDefault(u =>
                    u.Email == email);
        }

        public User GetByUserCode(
             string userCode)
        {
            return _dbSet
                .FirstOrDefault(u =>
                    u.UserCode == userCode);
        }
    }
}