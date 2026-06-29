using AutoMapper;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class AppointmentService(IAppointmentRepository repository, IHealthRecordService healthRecordService, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto entity, int patientId)
        {
            //(no past booking)
            if (entity.ScheduledDate < DateTime.UtcNow)
                throw new Exception("Cannot book appointment in the past");

            //Validate time format
            if (!TimeSpan.TryParse(entity.TimeSlot, out _))
                throw new Exception("Invalid time format. Use HH:mm");

            //Doctor availability check
            var doctorConflict = await repository.DoctorHasConflictAsync(
                entity.DoctorId,
                entity.ScheduledDate,
                entity.TimeSlot
            );

            if (doctorConflict)
                throw new Exception("Doctor already booked for this time slot");

            //Patient same-slot conflict
            var patientConflict = await repository.PatientHasConflictAsync(
                patientId,
                entity.ScheduledDate,
                entity.TimeSlot
            );

            if (patientConflict)
                throw new Exception("You already have an appointment at this time");

            //limit one appointment per day
            var dailyLimit = await repository.PatientHasAppointmentOnDateAsync(
                patientId,
                entity.ScheduledDate
            );

            if (dailyLimit)
                throw new Exception("You already have an appointment on this date");

            //Create entity
            var appointment = mapper.Map<Appointment>(entity);

            appointment.PatientId = patientId;

            //add status
            appointment.Status = "Pending";

            var savedEntity = await repository.CreateAsync(appointment);

            return mapper.Map<AppointmentDto>(savedEntity);
        }


        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var deleted = await repository.DeleteAsync(appointmentId);
            return deleted;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAllAsync());
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetUpcomingAppointmentsByDoctorIdAsync(doctorId));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByPatientIdAsync(patientId));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorNameAsync(string doctorName)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByDoctorNameAsync(doctorName));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientNameAsync(string patientName)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByPatientNameAsync(patientName));
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await repository.GetByIdAsync(id);
            return mapper.Map<AppointmentDto?>(appointment);
        }

        public async Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentStatusDto entity)
        {
            var existing = await repository.GetByIdAsync(id);

            if (existing == null)
                return null;

            var previousStatus = existing.Status;

            // ✅ BLOCK invalid base cases
            if (previousStatus == "Completed")
                throw new Exception("Completed appointment cannot be modified");

            if (previousStatus == "Cancelled")
                throw new Exception("Cancelled appointment cannot be modified");

            // ✅ VALID STATUS CHECK
            if (!new[] { "Pending", "Confirmed", "Cancelled", "Completed" }
                .Contains(entity.Status))
            {
                throw new Exception("Invalid status value");
            }

            // ✅ STATE TRANSITION RULES
            if (previousStatus == "Pending")
            {
                if (entity.Status != "Confirmed" && entity.Status != "Cancelled")
                    throw new Exception("Pending appointment can only be Confirmed or Cancelled");
            }

            if (previousStatus == "Confirmed")
            {
                if (entity.Status != "Completed" && entity.Status != "Cancelled")
                    throw new Exception("Confirmed appointment can only be Completed or Cancelled");
            }

            // ✅ CANCELLATION RULE
            if (entity.Status == "Cancelled")
            {
                if (string.IsNullOrWhiteSpace(entity.CancellationReason))
                    throw new Exception("Cancellation reason is required");

                existing.CancellationReason = entity.CancellationReason;
            }
            else
            {
                // ✅ clear reason if not cancelled
                existing.CancellationReason = null;
            }

            // ✅ UPDATE STATUS
            existing.Status = entity.Status;

            var updated = await repository.UpdateAsync(id, existing);

            // ✅ HEALTH RECORD CREATION (ONLY ON FIRST COMPLETION)
            if (previousStatus != "Completed" && entity.Status == "Completed")
            {
                await healthRecordService.CreateFromAppointment(existing);
            }

            return mapper.Map<AppointmentDto>(updated);
        }

    }
}
