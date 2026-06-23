using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> getAllActiveAsync()
        {
            var exiting = await _context.Set<Doctor>()
                .Where(a => a.IsActive == true)
                .ToListAsync();

            return exiting;
        }

        public async Task<List<Doctor>?> searchbyspecialisationAsync(string specialisation)
        {
            var exiting = await _context.Set<Doctor>()
                .Where(d => d.Specialisation == specialisation)
                .ToListAsync();

            return exiting;
        }

        public async Task<Doctor?> GetByIdentityUserIdAsync(string identityUserId)
        {
            return await _context.Set<Doctor>()
                .FirstOrDefaultAsync(d => d.IdentityUserId == identityUserId);
        }
    }
}