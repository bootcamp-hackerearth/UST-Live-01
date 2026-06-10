using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public class HealthRecordServiceImpl : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IMapper _mapper;

        public HealthRecordServiceImpl(
            IHealthRecordRepository repo,
            IAppointmentRepository appointmentRepo,
            IMapper mapper)
        {
            _repo = repo;
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
        }

        public ApiResponseDto Add(HealthRecordDto dto)
        {
            var appointment = _appointmentRepo.GetById(dto.AppointmentId);

            if (appointment == null)
                return new ApiResponseDto { Success = false, Message = "Invalid appointment" };

            if (appointment.Status != AppointmentStatus.Completed.ToString())
                return new ApiResponseDto { Success = false, Message = "Not completed" };

            if (_repo.ExistsByAppointment(dto.AppointmentId))
                return new ApiResponseDto { Success = false, Message = "Already exists" };

            var record = _mapper.Map<HealthRecord>(dto);
            record.VisitDate = DateTime.Now;

            _repo.Add(record);
            _repo.Save();

            return new ApiResponseDto { Success = true, Message = "Health record added" };
        }

        public List<HealthRecordDto> GetByPatient(int patientId)
        {
            return _mapper.Map<List<HealthRecordDto>>(_repo.GetByPatient(patientId));
        }

        public HealthRecordDto GetById(int recordId)
        {
            var record = _repo.GetById(recordId);
            return record == null ? null : _mapper.Map<HealthRecordDto>(record);
        }
    }
}