using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

public class AppointmentServiceImpl : IAppointmentService
{
    private readonly IAppointmentRepository _repo;

    public AppointmentServiceImpl(IAppointmentRepository repo)
    {
        _repo = repo;
    }

    public List<AppointmentDto> GetByPatient(int patientId)
    {
        return _repo.GetByPatient(patientId)
            .Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                ScheduledDate = a.ScheduledDate,
                TimeSlot = a.TimeSlot,
                CancellationReason = a.CancellationReason,
                CancelledBy = a.CancelledBy,

                Status = Enum.TryParse(a.Status, out AppointmentStatus s)
                    ? s : AppointmentStatus.Pending,

                DoctorName = a.Doctor.FullName
            }).ToList();
    }

    public List<AppointmentDto> GetByDoctor(int doctorId)
    {
        return _repo.GetByDoctor(doctorId)
            .Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                ScheduledDate = a.ScheduledDate,
                TimeSlot = a.TimeSlot,
                CancellationReason = a.CancellationReason,
                CancelledBy = a.CancelledBy,

                Status = Enum.TryParse(a.Status, out AppointmentStatus s)
                    ? s : AppointmentStatus.Pending,

                PatientName = a.Patient.FullName
            }).ToList();
    }

    public List<string> GetBookedSlots(int doctorId, DateTime date)
    {
        return _repo.GetBookedSlots(doctorId, date);
    }

    public List<string> GetSlots(int doctorId, DateTime date)
    {
        var allSlots = new List<string>
        {
            "10:00 AM : 11:00 AM",
            "11:00 AM : 12:00 PM",
            "1:00 PM : 2:00 PM",
            "2:00 PM : 3:00 PM"
        };

        var booked = _repo.GetBookedSlots(doctorId, date);

        return allSlots.Except(booked).ToList();
    }

    public ApiResponseDto Book(BookAppointmentDto dto)
    {
        if (string.IsNullOrEmpty(dto.TimeSlot))
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "Time slot required"
            };
        }

        if (dto.TimeSlot == "Select Doctor & Date")
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "Invalid time slot"
            };
        }

        if (dto.ScheduledDate < DateTime.Today)
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "Cannot book appointment in the past"
            };
        }

        if (_repo.ExistsSameDay(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "Already booked with this doctor for this day"
            };
        }

        if (_repo.IsSlotTaken(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "Slot already taken"
            };
        }

        var allSlots = new List<string>
        {
            "10:00 AM : 11:00 AM",
            "11:00 AM : 12:00 PM",
            "1:00 PM : 2:00 PM",
            "2:00 PM : 3:00 PM"
        };

        var booked = _repo.GetBookedSlots(dto.DoctorId, dto.ScheduledDate);

        if (booked.Count >= allSlots.Count)
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "No slots available for this doctor"
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

    public ApiResponseDto UpdateStatus(int id, UpdateAppointmentStatusDto dto)
    {
        var a = _repo.GetById(id);

        if (a == null)
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = "Not found"
            };
        }

        a.Status = dto.Status.ToString();

        if (dto.Status == AppointmentStatus.Cancelled)
        {
            if (string.IsNullOrEmpty(dto.CancellationReason))
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Cancellation reason required"
                };
            }

            a.CancellationReason = dto.CancellationReason;
            a.CancelledBy = dto.CancelledBy;
        }

        _repo.Update(a);
        _repo.Save();

        return new ApiResponseDto
        {
            Success = true,
            Message = "Status updated"
        };
    }
}

