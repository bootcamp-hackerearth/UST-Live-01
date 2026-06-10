using HealthCare.Shared;
//using HealthCareApi.Helper;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace HealthCareApi.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly HealthAppDbContext _context;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            HealthAppDbContext context)
        {
            _healthRecordRepository = healthRecordRepository;
            _context = context;
        }

        // ADD HEALTH RECORD
        public async Task<HealthRecord> AddHealthRecordAsync(HealthRecord record)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == record.AppointmentId);

            if (appointment == null)
                throw new Exception("Invalid appointment");

            // Allow only if NOT already completed? NO ❌
            // Check if already has record (UNIQUE constraint)
            var exists = await _healthRecordRepository
                .HealthRecordExistsAsync(record.AppointmentId);

            if (exists)
                throw new Exception("Health record already exists for this appointment");

            // NO need to check "Completed"

            record.VisitDate = DateTime.Now;

            await _healthRecordRepository.AddAsync(record);

            // Trigger will handle status update

            return record;
        }

        // GET PATIENT HEALTH HISTORY (VIEW)
        public async Task<PagedResult<vw_PatientHealthHistory>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _healthRecordRepository
                .GetPatientHealthHistoryAsync(patientId, pageNumber, pageSize);
        }
    }
}