using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>,IPatientRepository
    {
        public PatientRepository(HealthCareDbContext context) : base(context) { }

        //public async Task<bool> EmailExistsAsync(string email)
        //{
        //    if (email == null)
        //        return false;

        //    return await _context.Set<Patient>().AnyAsync(p => p.Email.ToLower() == email.ToLower());

        //}

    }
}
