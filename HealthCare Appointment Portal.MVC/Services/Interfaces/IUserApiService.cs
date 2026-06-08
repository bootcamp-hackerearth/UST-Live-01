using HealthCare_Appointment_Portal.DTOs.UserDtos;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services.Interfaces
{
    public interface IUserApiService
    {
        Task<UserDto>
            GetUserByCodeAsync(
                string userCode);
    }
}