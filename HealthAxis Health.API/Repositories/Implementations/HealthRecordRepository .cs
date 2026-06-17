using HealthAxisHealth.API.Data;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    [ExcludeFromCodeCoverage]
    public class HealthRecordRepository :
        Repository<HealthRecord>,
        IHealthRecordRepository
    {
        #region Constructor

        public HealthRecordRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<HealthRecord>> GetByPatientIdAsync(
            int patientId,
            CancellationToken cancellationToken = default)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Doctor)
                .Where(hr => hr.PatientId == patientId)
                .OrderByDescending(hr => hr.VisitDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HealthRecord>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken cancellationToken = default)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Where(hr => hr.DoctorId == doctorId)
                .OrderByDescending(hr => hr.VisitDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<HealthRecord?> GetHealthRecordWithDetailsAsync(
            int recordId,
            CancellationToken cancellationToken = default)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .FirstOrDefaultAsync(
                    hr => hr.RecordId == recordId,
                    cancellationToken);
        }

        public async Task<PagedResultDto<HealthRecord>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default)
        {
            IQueryable<HealthRecord> query = _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor);

            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                string search = pagination.Search.Trim();

                query = query.Where(hr =>
                    hr.Patient.FullName.Contains(search) ||
                    hr.Doctor.FullName.Contains(search) ||
                    hr.Diagnosis.Contains(search) ||
                    hr.Prescription.Contains(search));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            int pageNumber = pagination.PageNumber ?? 1;
            int pageSize = pagination.PageSize ?? 10;

            List<HealthRecord> records = await query
                .OrderByDescending(hr => hr.VisitDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResultDto<HealthRecord>
            {
                Items = records,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<HealthRecord?> GetByAppointmentIdAsync(
            int appointmentId,
            CancellationToken cancellationToken = default)
        {
            return await _context.HealthRecords
                .FirstOrDefaultAsync(
                    hr => hr.AppointmentId == appointmentId,
                    cancellationToken);
        }

        #endregion
    }
}