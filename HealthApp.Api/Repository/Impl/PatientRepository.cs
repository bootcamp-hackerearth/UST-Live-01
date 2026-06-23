using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{

    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly HealthAppDbContext _context;

        public PatientRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cd = default)
        {
            return await _context.Set<Patient>()
                .FirstOrDefaultAsync(p => p.IdentityUserId == identityUserId, cd);
        }
    }

}
