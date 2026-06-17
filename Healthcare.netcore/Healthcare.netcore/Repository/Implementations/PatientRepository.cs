using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;

namespace HealthAxis.API.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HealthAxisDbContext context) : base(context)
        {
        }
    }
}