using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class UserRepository
        : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User>
            GetByIdAsync(
                int id)
        {
            return await _context.Users
                .FindAsync(id);
        }

        public async Task<IEnumerable<User>>
            GetAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task AddAsync(
            User user)
        {
            _context.Users
                .Add(user);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            User user)
        {
            _context.Entry(user)
                .State = EntityState.Modified;

            await _context
                .SaveChangesAsync();
        }

        public async Task DeleteAsync(
            int id)
        {
            User user =
                await _context.Users
                    .FindAsync(id);

            if (user != null)
            {
                _context.Users
                    .Remove(user);

                await _context
                    .SaveChangesAsync();
            }
        }

        public User GetByEmail(
            string email)
        {
            return _context.Users
                .FirstOrDefault(u =>
                    u.Email == email);
        }

        public User GetByUserCode(
            string userCode)
        {
            return _context.Users
                .FirstOrDefault(u =>
                    u.UserCode == userCode);
        }
    }
}