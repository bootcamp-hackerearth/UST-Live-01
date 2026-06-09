using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository
            _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository =
                userRepository;
        }

        public async Task<User>
            GetByIdAsync(
                int id)
        {
            var user =
                await _userRepository
                    .GetByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(
                    $"User with Id {id} was not found.");
            }

            return user;
        }

        public Task<User>
            GetByUserCodeAsync(
                string userCode)
        {
            var user =
                _userRepository
                    .GetByUserCode(
                        userCode);

            if (user == null)
            {
                throw new UserNotFoundException(
                    $"User with code '{userCode}' was not found.");
            }

            return Task.FromResult(
                user);
        }

        public Task<User>
            GetByEmailAsync(
                string email)
        {
            var user =
                _userRepository
                    .GetByEmail(
                        email);

            if (user == null)
            {
                throw new UserNotFoundException(
                    $"User with email '{email}' was not found.");
            }

            return Task.FromResult(
                user);
        }
    }
}