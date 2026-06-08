using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users
                .GetByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(
                    $"User with Id {id} was not found.");
            }

            return user;
        }

        public async Task<User> GetByUserCodeAsync(
            string userCode)
        {
            var user = _unitOfWork.Users
                .GetByUserCode(userCode);

            if (user == null)
            {
                throw new UserNotFoundException(
                    $"User with code '{userCode}' was not found.");
            }

            return await Task.FromResult(user);
        }

        public async Task<User> GetByEmailAsync(
            string email)
        {
            var user = _unitOfWork.Users
                .GetByEmail(email);

            if (user == null)
            {
                throw new UserNotFoundException(
                    $"User with email '{email}' was not found.");
            }

            return await Task.FromResult(user);
        }
    }
}