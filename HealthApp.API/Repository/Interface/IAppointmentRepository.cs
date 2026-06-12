using HealthApp.API.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<Doctor> GetDoctorById(int doctorId);

        Task<bool> IsSlotBooked(int doctorId, DateTime date, string timeSlot);

        Task Add(Appointment appointment);

        Task Update(Appointment appointment);

        Task<List<Appointment>> GetAll();

        Task<Appointment> GetById(int id);

        Task<List<Appointment>> GetByPatient(int patientId);

        Task<List<Appointment>> GetUpcomingByDoctor(int doctorId, DateTime from, DateTime to);

        Task<List<Appointment>> GetPendingByDoctor(int doctorId);

        Task<List<string>> GetBookedSlots(int doctorId, DateTime date);
    }
}