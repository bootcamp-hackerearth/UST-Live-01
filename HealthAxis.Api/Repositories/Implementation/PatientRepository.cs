using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interface;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class PatientRepository : Repository<Patient>,IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
