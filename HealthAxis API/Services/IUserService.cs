using HealthAxis.API.DTOs.Users;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IUserService
        : IService<User, UserReadDto, UserCreateDto, UserUpdateDto>
    {
    }
}
