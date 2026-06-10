using HealthCare.Shared;
using HealthCare.Shared.DTOs.Appointment;
using HealthCare.Shared.DTOs.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services.Interfaces
{
    public interface IAppointmentService
    {

        Task<PagedResult<AppointmentDto>> GetPatientAppointmentsAsync(
                int patientId,
                string status,
                int pageNumber,
                int pageSize);

        Task<IEnumerable<AppointmentDto>> GetTodayAppointmentsAsync(int doctorId);
        Task<IEnumerable<AppointmentDto>> GetWeeklyAppointmentsAsync(int doctorId);
        Task<IEnumerable<AppointmentDto>> GetByDateAsync(DateTime date);

        Task<bool> BookAsync(AppointmentDto dto);
        Task<bool> ConfirmAsync(int id);
        Task<bool> CancelAsync(int id, string reason);
        //Task<List<DoctorDto>> GetAllAsync();
        Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date);

    }
}