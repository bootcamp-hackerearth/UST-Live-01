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
        Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<Appointment> ConfirmAppointmentAsync(int appointmentId);
        Task<Appointment> CancelAppointmentAsync(int appointmentId, string reason);
        Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date);
        

    }
}