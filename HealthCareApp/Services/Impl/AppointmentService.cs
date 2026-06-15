using AutoMapper;
using HealthCareApp.Constants;
using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;


namespace HealthCareApp.Services.Impl
{

    public class AppointmentService(

        IAppointmentRepository appointmentRepository,

        IPatientRepository patientRepository,

        IDoctorRepository doctorRepository,

        IMapper mapper) : IAppointmentService

{

    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()

    {

        var appointments = await appointmentRepository.GetAllAsync();

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)

    {

        var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

        if (appointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        return mapper.Map<AppointmentDto>(appointment);

    }

    public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)

    {

        var appointments = await appointmentRepository.GetByPatientIdAsync(patientId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)

    {

        var appointments = await appointmentRepository.GetByDoctorIdAsync(doctorId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status)

    {

        var appointments = await appointmentRepository.GetByStatusAsync(status);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync()

    {

        var appointments = await appointmentRepository.GetUpcomingAppointmentsAsync();

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByPatientIdAsync(int patientId)

    {

        var appointments = await appointmentRepository.GetUpcomingAppointmentsByPatientIdAsync(patientId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctorIdAsync(int doctorId)

    {

        var appointments = await appointmentRepository.GetUpcomingAppointmentsByDoctorIdAsync(doctorId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetPendingAppointmentsByPatientIdAsync(int patientId)

    {

        var appointments = await appointmentRepository.GetPendingAppointmentsByPatientIdAsync(patientId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetPendingAppointmentsByDoctorIdAsync(int doctorId)

    {

        var appointments = await appointmentRepository.GetPendingAppointmentsByDoctorIdAsync(doctorId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<List<AppointmentDto>> GetTodayConfirmedAppointmentsByDoctorIdAsync(int doctorId)

    {

        var appointments = await appointmentRepository.GetTodayConfirmedAppointmentsByDoctorIdAsync(doctorId);

        return mapper.Map<List<AppointmentDto>>(appointments);

    }

    public async Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto)

    {

        var patient = await patientRepository.GetByIdAsync(dto.PatientId);

        if (patient is null)

        {

            throw new Exception("Patient not found.");

        }

        var doctor = await doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor is null)

        {

            throw new Exception("Doctor not found.");

        }

        if (!doctor.IsActive)

        {

            throw new Exception("Doctor is inactive. Appointment cannot be booked.");

        }

        if (dto.ScheduledDate.Date < DateTime.Today)

        {

            throw new Exception("Appointment date cannot be in the past.");

        }

        if (!TimeSlots.All.Contains(dto.TimeSlot))

        {

            throw new Exception("Invalid time slot selected.");

        }

        var isSlotBooked = await appointmentRepository.IsSlotBookedAsync(

            dto.DoctorId,

            dto.ScheduledDate,

            dto.TimeSlot);

        if (isSlotBooked)

        {

            throw new Exception("This time slot is already booked for the selected doctor.");

        }

        var patientHasSameSlot = await appointmentRepository.PatientHasActiveAppointmentOnDateAndSlotAsync(

            dto.PatientId,

            dto.ScheduledDate,

            dto.TimeSlot);

        if (patientHasSameSlot)

        {

            throw new Exception("Patient already has an active appointment in this time slot.");

        }

        var patientHasAppointmentWithDoctor = await appointmentRepository.PatientHasActiveAppointmentWithDoctorOnDateAsync(

            dto.PatientId,

            dto.DoctorId,

            dto.ScheduledDate);

        if (patientHasAppointmentWithDoctor)

        {

            throw new Exception("Patient already has an active appointment with this doctor on the selected date.");

        }

        var appointment = mapper.Map<Appointment>(dto);

        appointment.Status = AppointmentStatus.Pending;

        appointment.CreatedDate = DateTime.Now;

        var savedAppointment = await appointmentRepository.CreateAsync(appointment);

        return mapper.Map<AppointmentDto>(savedAppointment);

    }

    public async Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto)

    {

        var existingAppointment = await appointmentRepository.GetByIdAsync(appointmentId);

        if (existingAppointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        var patient = await patientRepository.GetByIdAsync(dto.PatientId);

        if (patient is null)

        {

            throw new Exception("Patient not found.");

        }

        var doctor = await doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor is null)

        {

            throw new Exception("Doctor not found.");

        }

        if (!doctor.IsActive)

        {

            throw new Exception("Doctor is inactive. Appointment cannot be updated.");

        }

        if (dto.ScheduledDate.Date < DateTime.Today)

        {

            throw new Exception("Appointment date cannot be in the past.");

        }

        if (!TimeSlots.All.Contains(dto.TimeSlot))

        {

            throw new Exception("Invalid time slot selected.");

        }

        mapper.Map(dto, existingAppointment);

        existingAppointment.AppointmentId = appointmentId;

        var updatedAppointment = await appointmentRepository.UpdateAsync(appointmentId, existingAppointment);

        if (updatedAppointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        return mapper.Map<AppointmentDto>(updatedAppointment);

    }

    public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)

    {

        var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

        if (appointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        if (appointment.Status == AppointmentStatus.Cancelled)

        {

            throw new Exception("Cancelled appointment cannot be confirmed.");

        }

        if (appointment.Status == AppointmentStatus.Completed)

        {

            throw new Exception("Completed appointment cannot be confirmed again.");

        }

        appointment.Status = AppointmentStatus.Confirmed;

        appointment.CancellationReason = null;

        var updatedAppointment = await appointmentRepository.UpdateAsync(appointmentId, appointment);

        return mapper.Map<AppointmentDto>(updatedAppointment);

    }

    public async Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId)

    {

        var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

        if (appointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        if (appointment.Status == AppointmentStatus.Cancelled)

        {

            throw new Exception("Cancelled appointment cannot be completed.");

        }

        appointment.Status = AppointmentStatus.Completed;

        var updatedAppointment = await appointmentRepository.UpdateAsync(appointmentId, appointment);

        return mapper.Map<AppointmentDto>(updatedAppointment);

    }

    public async Task<AppointmentDto> CancelAppointmentAsync(CancelAppointmentDto dto)

    {

        var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

        if (appointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        if (appointment.Status == AppointmentStatus.Completed)

        {

            throw new Exception("Completed appointment cannot be cancelled.");

        }

        appointment.Status = AppointmentStatus.Cancelled;

        appointment.CancellationReason = dto.Reason;

        var updatedAppointment = await appointmentRepository.UpdateAsync(dto.AppointmentId, appointment);

        return mapper.Map<AppointmentDto>(updatedAppointment);

    }

    public async Task<AppointmentDto> DeleteAppointmentAsync(int appointmentId)

    {

        var deletedAppointment = await appointmentRepository.DeleteAsync(appointmentId);

        if (deletedAppointment is null)

        {

            throw new Exception("Appointment not found.");

        }

        return mapper.Map<AppointmentDto>(deletedAppointment);

    }

}

}

