using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;

public interface IAppointmentService
{
    List<AppointmentDto> GetByPatient(int patientId);
    List<AppointmentDto> GetByDoctor(int doctorId);

    ApiResponseDto Book(BookAppointmentDto dto);
    ApiResponseDto UpdateStatus(int id, UpdateAppointmentStatusDto dto);
    List<string> GetBookedSlots(int doctorId, DateTime date);
    List<string> GetSlots(int doctorId, DateTime date);
}