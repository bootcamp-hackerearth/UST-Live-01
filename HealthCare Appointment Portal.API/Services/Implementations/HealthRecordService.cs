using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class HealthRecordService
        : IHealthRecordService
    {
        private readonly IUnitOfWork
            _unitOfWork;

        private readonly IMapper
            _mapper;

        public HealthRecordService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork =
                unitOfWork;

            _mapper =
                mapper;
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetAllHealthRecordsAsync()
        {
            var records =
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .Appointments
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
                await _unitOfWork
                    .HealthRecords
                    .RecordExistsAsync(
                        dto.AppointmentId);

            if (recordExists)
            {
                throw new DuplicateHealthRecordException();
            }

            var record =
                _mapper.Map<HealthRecord>(
                    dto);

            await _unitOfWork
                .HealthRecords
                .AddAsync(
                    record);

            await _unitOfWork
                .CommitAsync();

            return record.RecordId;
        }

        public async Task
            UpdateHealthRecordAsync(
                int recordId,
                UpdateHealthRecordDto dto)
        {
            var record =
                await _unitOfWork
                    .HealthRecords
                    .GetByIdAsync(
                        recordId);

            if (record == null)
            {
                throw new HealthRecordNotFoundException();
            }

            _mapper.Map(
                dto,
                record);

            await _unitOfWork
                .HealthRecords
                .UpdateAsync(
                    record);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task
            DeleteHealthRecordAsync(
                int recordId)
        {
            var record =
                await _unitOfWork
                    .HealthRecords
                    .GetByIdAsync(
                        recordId);

            if (record == null)
            {
                throw new HealthRecordNotFoundException();
            }

            await _unitOfWork
                .HealthRecords
                .DeleteAsync(
                    recordId);

            await _unitOfWork
                .CommitAsync();
        }
    }
}