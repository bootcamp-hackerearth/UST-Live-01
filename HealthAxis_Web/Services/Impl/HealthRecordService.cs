using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAxis.Api.Services
{
    public class HealthRecordServiceImpl : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repo;
        private readonly IAppointmentRepository _appointmentRepo;

        public HealthRecordServiceImpl(IHealthRecordRepository repo,
                                       IAppointmentRepository appointmentRepo)
        {
            _repo = repo;
            _appointmentRepo = appointmentRepo;
        }

        public ApiResponseDto Create(CreateHealthRecordDto dto)
        {
            if (dto == null)
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Invalid data"
                };
            }

            if (_repo.ExistsByAppointment(dto.AppointmentId))
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Health record already exists"
                };
            }

            var appointment = _appointmentRepo.GetById(dto.AppointmentId);

            if (appointment == null)
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Invalid AppointmentId"
                };
            }

            var record = new HealthRecord
            {
                AppointmentId = dto.AppointmentId,

                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,

                VisitDate = DateTime.Now,

                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            _repo.Add(record);
            _repo.Save();

            appointment.Status = AppointmentStatus.Completed.ToString();
            _appointmentRepo.Update(appointment);
            _appointmentRepo.Save();

            return new ApiResponseDto
            {
                Success = true,
                Message = "Health record added successfully"
            };
        }
        public List<HealthRecordDto> GetByPatient(int patientId)
        {
            var records = _repo.GetByPatient(patientId);

            return records.Select(r => new HealthRecordDto
            {
                RecordId = r.RecordId,
                AppointmentId = r.AppointmentId,
                VisitDate = r.VisitDate,
                Diagnosis = r.Diagnosis,
                Prescription = r.Prescription,
                Notes = r.Notes
            }).ToList();
        }

    }
}