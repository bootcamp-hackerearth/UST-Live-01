using HealthAxis.API.Data;
using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HealthAxisDbContext context)
            : base(context)
        {
        }
    }
}
