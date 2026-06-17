using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    [ExcludeFromCodeCoverage]
    public class PatientRepository :
        Repository<Patient>,
        IPatientRepository
    {
        #region Constructor

        public PatientRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        #endregion

        #region Methods

        public async Task<Patient?> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.UserId == userId,
                    cancellationToken);
        }

        public async Task<Patient?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.Email == email,
                    cancellationToken);
        }

        public async Task<Patient?> GetPatientWithHealthRecordsAsync(
            int patientId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .Include(p => p.HealthRecords)
                .FirstOrDefaultAsync(
                    p => p.PatientId == patientId,
                    cancellationToken);
        }

        public async Task<PagedResultDto<Patient>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Patient> query = _context.Patients;

            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                string search = pagination.Search.Trim();

                query = query.Where(p =>
                    p.FullName.Contains(search) ||
                    p.Email.Contains(search) ||
                    p.PhoneNumber.Contains(search));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            int pageNumber = pagination.PageNumber ?? 1;
            int pageSize = pagination.PageSize ?? 10;

            List<Patient> patients = await query
                .OrderBy(p => p.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResultDto<Patient>
            {
                Items = patients,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}