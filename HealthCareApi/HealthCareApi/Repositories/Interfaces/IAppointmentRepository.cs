using HealthCare.Shared;
//using HealthCareApi.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCareApi.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<bool> SlotExistsAsync(int doctorId, string timeSlot);
        Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot);
        Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
         int patientId,
         string status,
         int pageNumber,
         int pageSize);
        Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
    }
}