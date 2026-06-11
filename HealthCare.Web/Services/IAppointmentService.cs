using HealthCare.Shared;
using HealthCare.Shared.DTOs.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services.Interfaces
{
    public interface IAppointmentService
    {

        Task<IEnumerable<AppointmentDto>> GetTodayAppointmentsAsync(int doctorId);
        Task<IEnumerable<AppointmentDto>> GetWeeklyAppointmentsAsync(int doctorId);
        Task<IEnumerable<AppointmentDto>> GetByDateAsync(DateTime date);
        Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date);


        Task<bool> BookAsync(AppointmentDto dto);
        Task<bool> ConfirmAsync(int id);
        Task<bool> CancelAsync(int id, string reason);
        Task<PagedResult<AppointmentDto>> GetUpcomingAppointmentsAsync(
            int? patientId,
            int? doctorId,
            string status,
            int pageNumber,
            int pageSize);
        Task<PagedResult<AppointmentDto>> GetAppointmentsAsync(
            int? patientId,
            int? doctorId,
            string status,
            int pageNumber,
            int pageSize);

    }
}