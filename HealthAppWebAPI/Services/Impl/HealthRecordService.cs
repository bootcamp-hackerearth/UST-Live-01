using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Interfaces;
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
            return _mapper.Map<List<HealthRecordDto>>(records);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);

            if (record == null)
                throw new Exception("Health Record not found.");

            return _mapper.Map<HealthRecordDto>(record);
        }

        public async Task AddAsync(CreateHealthRecordDto dto)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(dto.AppointmentId);

            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Confirmed.ToString())
            {
                throw new Exception(
                    "Health record can be added only for confirmed appointments.");
            }

            var existing = await _recordRepo
                .GetByAppointmentIdAsync(dto.AppointmentId);

            if (existing != null)
            {
                throw new Exception(
                    "Health record already exists for this appointment.");
            }

            var record = new HealthRecord
            {
                AppointmentId = dto.AppointmentId,

                // Important fix
                PatientId = appointment.PatientId,

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
            var records =
                await _recordRepo.GetByPatientIdAsync(patientId);

            return records
                .Select(h => new HealthRecordDto
                {
                    HealthRecordId = h.HealthRecordId,

                    AppointmentId = h.AppointmentId,

                    PatientId =
                        h.Appointment != null
                            ? h.Appointment.PatientId.GetValueOrDefault()
                            : 0,

                    VisitDate = h.VisitDate,

                    PatientName =
                        h.Appointment != null &&
                        h.Appointment.Patient != null
                            ? h.Appointment.Patient.FullName
                            : "",

                    DoctorName =
                        h.Appointment != null &&
                        h.Appointment.Doctor != null
                            ? h.Appointment.Doctor.FullName
                            : "",

                    Diagnosis = h.Diagnosis,

                    Prescription = h.Prescription,

                    Notes = h.Notes
                })
                .ToList();
        }
        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var records = await _recordRepo.GetByPatientIdAsync(patientId);

            return _mapper.Map<List<HealthRecordDto>>(records);
        }

        public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId)
        {
            var record = await _recordRepo.GetByAppointmentIdAsync(appointmentId);

            return record != null;
        }

    }
}