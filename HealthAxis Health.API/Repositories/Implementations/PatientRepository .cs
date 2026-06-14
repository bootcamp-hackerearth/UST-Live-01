using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisHealth.API.Repositories.Implementations
{
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
            int userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.UserId == userId);
        }

        public async Task<Patient?> GetByEmailAsync(
            string email)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.Email == email);
        }

        public async Task<Patient?>
            GetPatientWithHealthRecordsAsync(
                int patientId)
        {
            return await _context.Patients
                .Include(p => p.HealthRecords)
                .FirstOrDefaultAsync(
                    p => p.PatientId == patientId);
        }

        public async Task<PagedResultDto<Patient>>
            GetPagedAsync(
                PaginationParams pagination)
        {
            IQueryable<Patient> query =
                _context.Patients;

            if (!string.IsNullOrWhiteSpace(
                pagination.Search))
            {
                string search =
                    pagination.Search.Trim();

                query = query.Where(p =>
                    p.FullName.Contains(search) ||
                    p.Email.Contains(search) ||
                    p.PhoneNumber.Contains(search));
            }

            int totalRecords =
                await query.CountAsync();

            List<Patient> patients =
                await query
                    .OrderBy(p => p.FullName)
                    .Skip(
                        (pagination.PageNumber - 1)
                        * pagination.PageSize)
                    .Take(
                        pagination.PageSize)
                    .ToListAsync();

            return new PagedResultDto<Patient>
            {
                Items = patients,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}
