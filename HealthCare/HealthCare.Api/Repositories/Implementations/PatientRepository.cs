using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>,IPatientRepository
    {
  
        public PatientRepository(HealthCareDbContext context) : base(context) { }
        public async Task<Patient?> GetByUserIdAsync(string userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }


    }
}
