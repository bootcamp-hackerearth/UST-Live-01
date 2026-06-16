using AutoMapper;
using HealthAxis.API.DTOs.Users;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class UserService
        : Service<User, UserReadDto, UserCreateDto, UserUpdateDto>, IUserService
    {
        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
            : base(userRepository, mapper)
        {
        }
    }
}
