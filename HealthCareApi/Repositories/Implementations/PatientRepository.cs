using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HealthCareApi.Repositories.Interfaces;
using HealthCare.Shared;

namespace HealthCareApi.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HealthAppDbContext _context) : base(_context)
        {
        }

        public async Task<PagedResult<Patient>> GetPaginatedPatientsAsync(
            string searchTerm,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 50 ? 50 : pageSize;

            IQueryable<Patient> query = _context.Patients
                .Where(p => p.IsActive);

            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p =>
                    p.FullName.ToLower().Contains(searchTerm.ToLower()));
            }

            // Get total BEFORE pagination
            int totalCount = await query.CountAsync();

            // Sorting
            query = query.OrderBy(p => p.PatientId);

            // Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return full result
            return new PagedResult<Patient>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public override async Task DeleteAsync(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            await _context.SaveChangesAsync();
        }
    }
}