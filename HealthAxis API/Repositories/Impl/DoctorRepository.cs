using HealthAxis.API.Data;
using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HealthAxisDbContext context)
            : base(context)
        {
        }
    }
}
