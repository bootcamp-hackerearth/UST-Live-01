using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;

namespace HealthApp.API.Repository.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HealthAppDbContext context) : base(context)
        {
        }
    }
}
