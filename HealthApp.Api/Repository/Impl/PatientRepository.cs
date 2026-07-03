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

        public async Task<Patient?> GetByIdentityUserIdAsync(string identityUserId, 
            CancellationToken cd = default)
        {
            return await _context.Set<Patient>()
                .FirstOrDefaultAsync(p => p.IdentityUserId == identityUserId, cd);
        }


        public async Task<(List<Patient> Items, int TotalCount)> GetPagedPatientsAsync(int pageNumber, 
            int pageSize, string? search = null, CancellationToken cd = default)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {

                query = query.Where(p =>
                    (p.FullName != null && p.FullName.Contains(search)) ||
                    (p.Email != null && p.Email.Contains(search)) ||
                    p.PatientId.ToString().Contains(search));

            }

            var totalCount = await query.CountAsync(cd);

            var items = await query
                .OrderBy(p => p.PatientId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cd);

            return (items, totalCount);
        }


        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cd = default)
        {
            var query = _context.Patients.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.PatientId != excludeId.Value);
            }


            return await query.AnyAsync(p =>
                p.Email != null &&
                string.Equals(
                    p.Email,
                    email,
                    StringComparison.OrdinalIgnoreCase),
                cd);

        }
    }

}
