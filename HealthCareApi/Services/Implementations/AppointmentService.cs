using HealthCare.Shared;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
using HealthCareWebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCareApi.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly HealthAppDbContext _context;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            HealthAppDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _context = context;
        }

        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            // 1. Validate patient
            var patientExists = await _appointmentRepository.PatientExistsAsync(appointment.PatientId);
            //var patientExists = await _context.Patients.AnyAsync(p =>
            //    p.PatientId == appointment.PatientId);

            if (!patientExists)
                throw new Exception("Invalid or inactive patient");

            // 2. Validate doctor
            var doctorExists = await _appointmentRepository.DoctorExistsAsync(appointment.DoctorId);
            //var doctorExists = await _context.Doctors.AnyAsync(d =>
            //    d.DoctorId == appointment.DoctorId && d.IsActive);

            if (!doctorExists)
                throw new Exception("Invalid or inactive doctor");

            // 3. Validate timeslot exists
            var slotExists = await _appointmentRepository
                .SlotExistsAsync(appointment.DoctorId, appointment.TimeSlot);

            if (!slotExists)
                throw new Exception("Invalid time slot for this doctor");

            // 4. Prevent booking in the past
            if (appointment.ScheduledDate.Date < DateTime.Today)
                throw new Exception("Cannot book appointment in the past");

            // 5. Check slot availability
            var isBooked = await _appointmentRepository
                .IsSlotBookedAsync(appointment.DoctorId,
                                   appointment.ScheduledDate,
                                   appointment.TimeSlot);

            if (isBooked)
                throw new Exception("Time slot already booked");

            // 6. Set default status
            appointment.Status = "Pending";

            // 7. Save
            await _appointmentRepository.AddAsync(appointment);

            return appointment;
        }

        public async Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
            int patientId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _appointmentRepository.GetPatientAppointmentsAsync(
                patientId,
                status,
                pageNumber,
                pageSize);
        }

        public async Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(
            int doctorId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _appointmentRepository.GetDoctorAppointmentsAsync(
                doctorId,
                status,
                pageNumber,
                pageSize);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _appointmentRepository.GetAppointmentsByDateAsync(date);
        }

        public async Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            //  Step 1: Get working slots
            var allSlots = await _appointmentRepository
                .GetDoctorSlotsAsync(doctorId);

            //  Step 2: Get booked slots
            var bookedSlots = await _appointmentRepository
                .GetBookedSlotsAsync(doctorId, date);

            //  Step 3: Remove booked slots
            var availableSlots = allSlots
                .Except(bookedSlots)
                .ToList();

            return availableSlots;
        }


        public async Task<Appointment> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status == "Cancelled")
                throw new Exception("Cannot confirm a cancelled appointment");

            if (appointment.Status == "Completed")
                throw new Exception("Appointment already completed");

            // Update status
            appointment.Status = "Confirmed";

            await _appointmentRepository.UpdateAsync(appointment);

            return appointment;
        }

        public async Task<Appointment> CancelAppointmentAsync(int appointmentId, string reason)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status == "Cancelled")
                throw new Exception("Appointment already cancelled");

            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Cancellation reason is required");

            // Update fields
            appointment.Status = "Cancelled";
            appointment.CancellationReason = reason;

            await _appointmentRepository.UpdateAsync(appointment);

            return appointment;
        }

        public async Task<PagedResult<Appointment>> GetUpcomingAppointmentsAsync(
    int? patientId,
    int? doctorId,
    int pageNumber,
    int pageSize)
        {
            return await _appointmentRepository.GetUpcomingAppointmentsAsync(
                patientId,
                doctorId,
                pageNumber,
                pageSize);
        }
    }
}