using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetByIdAsync(
            int id);

        Task<IEnumerable<Appointment>> GetAllAsync();

        Task AddAsync(
            Appointment appointment);

        Task UpdateAsync(
            Appointment appointment);

        Task DeleteAsync(
            int id);

        Task<IEnumerable<Appointment>>
            GetAppointmentsByPatientAsync(
                int patientId);

        Task<IEnumerable<Appointment>>
            GetAppointmentsByDoctorAsync(
                int doctorId);

        Task<IEnumerable<Appointment>>
            GetUpcomingAppointmentsAsync();

        Task<IEnumerable<Appointment>>
            GetCompletedAppointmentsAsync();

        Task<Appointment>
            GetConflictingAppointmentAsync(
                int doctorId,
                DateTime date,
                string timeSlot);

        Task<IEnumerable<Appointment>>
            GetTodayScheduleAsync(
                int doctorId);

        Task<IEnumerable<Appointment>>
            GetWeeklyScheduleAsync(
                int doctorId);

        Task<Appointment>
            GetNextAppointmentByPatientAsync(
                int patientId);

        Task<bool>
            IsSlotAvailableAsync(
                int doctorId,
                DateTime date,
                string timeSlot);
    }
}