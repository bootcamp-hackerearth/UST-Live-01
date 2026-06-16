using HealthAxisHealth.API.Data;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    public class DoctorRepository :
        Repository<Doctor>,
        IDoctorRepository
    {
        #region Constructor

        public DoctorRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Doctors
                .Where(d => d.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialisationAsync(
            Specialisation specialisation,
            CancellationToken cancellationToken = default)
        {
            return await _context.Doctors
                .Where(d =>
                    d.Specialisation == specialisation &&
                    d.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<Doctor?> GetDoctorWithAppointmentsAsync(
            int doctorId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(
                    d => d.DoctorId == doctorId,
                    cancellationToken);
        }

        public async Task<Doctor?> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(
                    d => d.UserId == userId,
                    cancellationToken);
        }

        public async Task<PagedResultDto<Doctor>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                string search = pagination.Search.Trim();

                query = query.Where(d =>
                    d.FullName.Contains(search) ||
                    d.Specialisation.ToString().Contains(search));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            List<Doctor> doctors = await query
                .OrderBy(d => d.FullName)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResultDto<Doctor>
            {
                Items = doctors,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}