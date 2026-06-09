using AutoMapper;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class HealthRecordService
        : IHealthRecordService
    {
        private readonly IHealthRecordRepository
            _healthRecordRepository;

        private readonly IAppointmentRepository
            _appointmentRepository;

        private readonly ApplicationDbContext
            _context;

        private readonly IMapper
            _mapper;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IAppointmentRepository appointmentRepository,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _healthRecordRepository =
                healthRecordRepository;

            _appointmentRepository =
                appointmentRepository;

            _context =
                context;

            _mapper =
                mapper;
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetAllHealthRecordsAsync()
        {
            var records =
                await _healthRecordRepository
                    .GetAllRecordsWithDetailsAsync();

            return _mapper.Map<
                IEnumerable<HealthRecordDto>>(
                    records);
        }

        public async Task<HealthRecordDto>
            GetHealthRecordByIdAsync(
                int recordId)
        {
            var record =
                await _healthRecordRepository
                    .GetByIdAsync(
                        recordId);

            if (record == null)
            {
                throw new HealthRecordNotFoundException();
            }

            return _mapper.Map<
                HealthRecordDto>(
                    record);
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetRecordsByPatientAsync(
                int patientId)
        {
            var records =
                await _healthRecordRepository
                    .GetRecordsByPatientAsync(
                        patientId);

            return _mapper.Map<
                IEnumerable<HealthRecordDto>>(
                    records);
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetRecordsByDoctorAsync(
                int doctorId)
        {
            var records =
                await _healthRecordRepository
                    .GetRecordsByDoctorAsync(
                        doctorId);

            return _mapper.Map<
                IEnumerable<HealthRecordDto>>(
                    records);
        }

        public async Task<int>
            AddHealthRecordAsync(
                CreateHealthRecordDto dto)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        dto.AppointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Completed)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                            .CompletedHealthRecord);
            }

            bool recordExists =
                await _healthRecordRepository
                    .RecordExistsAsync(
                        dto.AppointmentId);

            if (recordExists)
            {
                throw new DuplicateHealthRecordException();
            }

            var record =
                _mapper.Map<HealthRecord>(
                    dto);

            await _healthRecordRepository
                .AddAsync(
                    record);

            await _context
                .SaveChangesAsync();

            return record.RecordId;
        }

        public async Task
            UpdateHealthRecordAsync(
                int recordId,
                UpdateHealthRecordDto dto)
        {
            var record =
                await _healthRecordRepository
                    .GetByIdAsync(
                        recordId);

            if (record == null)
            {
                throw new HealthRecordNotFoundException();
            }

            _mapper.Map(
                dto,
                record);

            await _healthRecordRepository
                .UpdateAsync(
                    record);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            DeleteHealthRecordAsync(
                int recordId)
        {
            var record =
                await _healthRecordRepository
                    .GetByIdAsync(
                        recordId);

            if (record == null)
            {
                throw new HealthRecordNotFoundException();
            }

            await _healthRecordRepository
                .DeleteAsync(
                    recordId);

            await _context
                .SaveChangesAsync();
        }
    }
}