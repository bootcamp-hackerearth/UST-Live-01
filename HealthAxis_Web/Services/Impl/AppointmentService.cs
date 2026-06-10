using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public class AppointmentServiceImpl : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IPatientRepository _patientRepo;
        private readonly IHealthRecordRepository _healthRepo;
        private readonly IMapper _mapper;

        public AppointmentServiceImpl(
            IAppointmentRepository repo,
            IPatientRepository patientRepo,
            IHealthRecordRepository healthRepo,
            IMapper mapper)
        {
            _repo = repo;
            _patientRepo = patientRepo;
            _healthRepo = healthRepo;
            _mapper = mapper;
        }

        public List<AppointmentDto> GetByDoctor(int doctorId)
        {
            var data = _repo.GetByDoctor(doctorId);
            var result = _mapper.Map<List<AppointmentDto>>(data);

            foreach (var a in result)
            {
                a.CanConfirm = a.Status == AppointmentStatus.Pending;
                a.CanComplete = a.Status == AppointmentStatus.Confirmed;
                a.CanCancel = a.CanConfirm || a.CanComplete;

                a.CanAddHealthRecord =
                    a.Status == AppointmentStatus.Completed &&
                    !_healthRepo.ExistsByAppointment(a.AppointmentId);
            }

            return result;
        }

        public List<AppointmentDto> GetByPatient(int patientId)
        {
            return _mapper.Map<List<AppointmentDto>>(_repo.GetByPatient(patientId));
        }

        public AppointmentDto GetById(int id)
        {
            var a = _repo.GetById(id);
            return a == null ? null : _mapper.Map<AppointmentDto>(a);
        }

        public ApiResponseDto Book(BookAppointmentDto dto)
        {
            if (dto.ScheduledDate < DateTime.Today)
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Past date not allowed"
                };
            }

            var patient = _patientRepo.GetById(dto.PatientId);
            if (patient == null || !patient.IsActive)
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Invalid or inactive patient"
                };
            }

            if (_repo.ExistsSameDay(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Already booked with this doctor on same day"
                };
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = AppointmentStatus.Pending.ToString()
            };

            _repo.Add(appointment);
            _repo.Save();

            return new ApiResponseDto
            {
                Success = true,
                Message = "Appointment booked successfully"
            };
        }

        public ApiResponseDto UpdateStatus(int id, AppointmentStatus status)
        {
            var a = _repo.GetById(id);
            if (a == null)
                return new ApiResponseDto { Success = false, Message = "Not found" };

            AppointmentStatus current;
            Enum.TryParse(a.Status, out current);

            if (status == AppointmentStatus.Confirmed && current != AppointmentStatus.Pending)
                return new ApiResponseDto { Success = false, Message = "Only pending → confirmed" };

            if (status == AppointmentStatus.Completed && current != AppointmentStatus.Confirmed)
                return new ApiResponseDto { Success = false, Message = "Only confirmed → completed" };

            a.Status = status.ToString();

            _repo.Update(a);
            _repo.Save();

            return new ApiResponseDto
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }

        public ApiResponseDto Cancel(int id, CancelAppointmentDto dto)
        {
            var a = _repo.GetById(id);
            if (a == null)
                return new ApiResponseDto { Success = false, Message = "Not found" };

            var current = (AppointmentStatus)
                Enum.Parse(typeof(AppointmentStatus), a.Status);

            if (current != AppointmentStatus.Pending &&
                current != AppointmentStatus.Confirmed)
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Only pending or confirmed appointments can be cancelled"
                };
            }

            a.Status = AppointmentStatus.Cancelled.ToString();
            a.CancellationReason = dto.CancellationReason;

            _repo.Update(a);
            _repo.Save();

            return new ApiResponseDto
            {
                Success = true,
                Message = "Appointment cancelled successfully"
            };
        }
    }
}