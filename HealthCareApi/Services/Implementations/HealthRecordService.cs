using HealthCare.Shared;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareApi.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IAppointmentRepository appointmentRepository
            )
        {
            _healthRecordRepository = healthRecordRepository;
            _appointmentRepository = appointmentRepository;
            
        }


        // ADD HEALTH RECORD
        public async Task<HealthRecord> AddHealthRecordAsync(HealthRecord record)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(record.AppointmentId);

            if (appointment == null)
                throw new KeyNotFoundException("Invalid appointment");

            
            var exists = await _healthRecordRepository
                .HealthRecordExistsAsync(record.AppointmentId);

            if (exists)
                throw new InvalidOperationException("Health record already exists for this appointment");

            // NO need to check "Completed"

            record.VisitDate = appointment.ScheduledDate;

            await _healthRecordRepository.AddAsync(record);

            // Trigger will handle status update

            return record;
        }

        // GET PATIENT HEALTH HISTORY 
        public async Task<PagedResult<vw_PatientHealthHistory>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _healthRecordRepository
                .GetPatientHealthHistoryAsync(patientId, pageNumber, pageSize);
        }

        public async Task<HealthRecord> GetByAppointmentIdAsync(int id)
        {
            return await _healthRecordRepository.GetByAppointmentIdAsync(id);
        }

        public async Task<HealthRecord> GetByIdAsync(int id)
        {
            return await _healthRecordRepository.GetByIdAsync(id);
        }
    }
}