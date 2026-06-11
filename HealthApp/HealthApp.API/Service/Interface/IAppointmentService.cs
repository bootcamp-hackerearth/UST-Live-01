using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Interface
{
    public interface IAppointmentService
    {
        Task Add(AppointmentDto dto);

        Task CancelAppointment(int appointmentId, string reason);
        Task<AppointmentDto> GetAppointmentById(int id);
        Task CompleteAppointment(int appointmentId);

        Task<List<AppointmentDto>> GetAllAppointments();
        Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId);
        Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date);
        Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(int doctorId, DateTime fromDate, DateTime toDate);
        Task<List<AppointmentDto>> GetPendingAppointmentsByDoctor(int doctorId);
        Task ConfirmAppointment(int appointmentId);
    }
}