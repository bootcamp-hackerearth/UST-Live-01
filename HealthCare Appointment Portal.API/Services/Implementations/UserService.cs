using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Services
{
    public class UserService
        : IUserService
    {
        private readonly IUnitOfWork
            _unitOfWork;

        public UserService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User>
            GetByIdAsync(int id)
        {
            return await _unitOfWork.Users
                .GetByIdAsync(id);
        }

        public async Task<User>
            GetByUserCodeAsync(
                string userCode)
        {
            return await Task.FromResult(
                _unitOfWork.Users
                    .GetByUserCode(userCode));
        }

        public async Task<User>
            GetByEmailAsync(
                string email)
        {
            return await Task.FromResult(
                _unitOfWork.Users
                    .GetByEmail(email));
        }
    }
}