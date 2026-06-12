using HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCareApi.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> BookAppointmentAsync(Appointment appointment);
        Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
            int patientId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(
            int doctorId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date);
        Task<Appointment> ConfirmAppointmentAsync(int appointmentId);
        Task<Appointment> CancelAppointmentAsync(int appointmentId, string reason);
        Task<PagedResult<Appointment>> GetUpcomingAppointmentsAsync(
            int? patientId,
            int? doctorId,
            int pageNumber,
            int pageSize);
    }
}