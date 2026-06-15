using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;

namespace HealthCareApp.Repository.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HealthAxisDbContext context) : base(context)
        {
        }
    }
}