using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}