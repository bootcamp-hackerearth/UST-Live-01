using HealthAxis.API.Data;
using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(HealthAxisDbContext context)
            : base(context)
        {

        }
    }
}

