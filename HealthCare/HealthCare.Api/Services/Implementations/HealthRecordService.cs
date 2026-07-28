using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.HealthRecord;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.HealthRecord;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        public HealthRecordService(
            IHealthRecordRepository repository,
            HealthCareDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<HealthRecordListDto?> GetByIdAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record is null)
                throw new InvalidOperationException("Health Record not found.");

            return _mapper.Map<HealthRecordListDto>(record);
        }

        public async Task<PagedResult<HealthRecordListDto>> GetAllAsync(HealthRecordFilter filter)
        {
            Expression<Func<HealthRecord, bool>>? predicate = null;

            if (filter.VisitDate.HasValue)
            {
                var start = filter.VisitDate.Value.ToDateTime(TimeOnly.MinValue);
                var end = start.AddDays(1);

                predicate = hr => hr.VisitDate >= start && hr.VisitDate < end;
            }

            Func<IQueryable<HealthRecord>, IOrderedQueryable<HealthRecord>> orderBy =
                q => q.OrderBy(hr => hr.VisitDate);

            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            return new PagedResult<HealthRecordListDto>
            {
                Items = _mapper.Map<IEnumerable<HealthRecordListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task AddAsync(int doctorId, CreateHealthRecordDto dto)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId);

            if (appointment == null)
                throw new InvalidOperationException("Appointment not found.");

            if (appointment.DoctorId != doctorId)
                throw new UnauthorizedAccessException("You cannot add record for this appointment.");

            if (appointment.Status != "Confirmed")
                throw new InvalidOperationException("Health record can be added only for confirmed appointments.");

            var existingRecord = await _context.HealthRecords
                .FirstOrDefaultAsync(r => r.AppointmentId == dto.AppointmentId);

            if (existingRecord != null)
                throw new InvalidOperationException("Health record already exists for this appointment.");

            var record = new HealthRecord
            {
                AppointmentId = dto.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = doctorId,

                VisitDate = appointment.ScheduledDate
        .ToDateTime(TimeOnly.MinValue),

                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            await _repository.AddAsync(record);

            // Important: mark appointment as completed after adding health record
            appointment.Status = "Completed";

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateHealthRecordDto dto)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record is null)
                throw new InvalidOperationException("Health record not found.");

            _mapper.Map(dto, record);

            await _repository.UpdateAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record is null)
                throw new InvalidOperationException("Health record not found.");

            try
            {
                await _repository.DeleteAsync(id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to delete health record.", ex);
            }
        }

        public async Task<List<HealthRecordListDto>> GetHealthRecordByPatient(int id)
        {
            return await _repository.GetHealthRecordByPatient(id);
        }

        public async Task<List<HealthRecordListDto>> GetHealthRecordByAppointment(int id)
        {
            var records = await _repository.GetHealthRecordByAppointment(id);

            return records.Count == 0
                ? new List<HealthRecordListDto>()
                : records;
        }
    }
}
