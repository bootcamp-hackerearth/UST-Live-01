using AutoMapper;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _recordRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IMapper _mapper;

        public HealthRecordService(
            IHealthRecordRepository recordRepo,
            IAppointmentRepository appointmentRepo,
            IMapper mapper)
        {
            _recordRepo = recordRepo;
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            var records = await _recordRepo.GetAllAsync();

            return records
                .Select(MapToDto)
                .ToList();
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);

            if (record == null)
            {
                throw new KeyNotFoundException("Health record not found.");
            }

            return MapToDto(record);
        }

        public async Task AddAsync(CreateHealthRecordDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Health record data is required.");
            }

            if (dto.AppointmentId <= 0)
            {
                throw new ArgumentException("Valid appointment is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
            {
                throw new ArgumentException("Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Prescription))
            {
                throw new ArgumentException("Prescription is required.");
            }

            var appointment = await _appointmentRepo.GetByIdAsync(dto.AppointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Confirmed.ToString())
            {
                throw new InvalidOperationException(
                    "Health record can be added only for confirmed appointments.");
            }

            var existing = await _recordRepo.GetByAppointmentIdAsync(dto.AppointmentId);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    "Health record already exists for this appointment.");
            }

            var record = new HealthRecord
            {
                AppointmentId = dto.AppointmentId,
                VisitDate = DateTime.Now,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            await _recordRepo.AddAsync(record);

            appointment.Status = AppointmentStatus.Completed.ToString();

            await _appointmentRepo.UpdateAsync(appointment);
        }

        public async Task<List<HealthRecordDto>> GetPatientHistoryAsync(int patientId)
        {
            if (patientId <= 0)
            {
                throw new ArgumentException("Valid patient is required.");
            }

            var records = await _recordRepo.GetByPatientIdAsync(patientId);

            return records
                .Select(MapToDto)
                .ToList();
        }

        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            if (patientId <= 0)
            {
                throw new ArgumentException("Valid patient is required.");
            }

            var records = await _recordRepo.GetByPatientIdAsync(patientId);

            return records
                .Select(MapToDto)
                .ToList();
        }

        public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new ArgumentException("Valid appointment is required.");
            }

            var record = await _recordRepo.GetByAppointmentIdAsync(appointmentId);

            return record != null;
        }

        private HealthRecordDto MapToDto(HealthRecord record)
        {
            return new HealthRecordDto
            {
                HealthRecordId = record.HealthRecordId,

                AppointmentId = record.AppointmentId,

                PatientId =
                    record.Appointment != null &&
                    record.Appointment.PatientId.HasValue
                        ? record.Appointment.PatientId.Value
                        : 0,

                VisitDate = record.VisitDate,

                PatientName =
                    record.Appointment != null &&
                    record.Appointment.Patient != null
                        ? record.Appointment.Patient.FullName
                        : "",

                DoctorName =
                    record.Appointment != null &&
                    record.Appointment.Doctor != null
                        ? record.Appointment.Doctor.FullName
                        : "",

                Diagnosis = record.Diagnosis,

                Prescription = record.Prescription,

                Notes = record.Notes
            };
        }
    }
}