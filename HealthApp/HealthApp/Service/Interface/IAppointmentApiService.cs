using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Service.Interface
{
    public interface IAppointmentApiService
    {
        Task<List<AppointmentDto>> GetAll();

        Task<AppointmentDto> GetById(int id);

        Task<List<AppointmentDto>> GetByPatient(int patientId);

        Task Create(AppointmentDto dto);

        Task MarkCompleted(int id);

        Task Confirm(int id);

        Task Cancel(int id, string reason);

        Task<List<string>> CheckAvailability(int doctorId, DateTime date);

        Task<PatientLookupDto> GetPatient(int id);

        Task<List<DoctorLookupDto>> GetDoctors(string specialization);

        Task<List<string>> GetAvailableSlots(int doctorId, DateTime date);
    }
}
