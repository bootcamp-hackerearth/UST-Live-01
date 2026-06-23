using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HealthApp.API.Service.Impl;

public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper) : IAppointmentService
{
    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
    {
        if (IsPatient())
        {
            var patient = await GetLoggedInPatientAsync();

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByPatientIdAsync(patient.PatientId));
        }

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetAllAsync());
    }
    public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
    {
        ValidateAppointmentId(appointmentId);

        var appointment = await appointmentRepository.GetByIdWithDetailsAsync(appointmentId)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        await EnsureAppointmentAccessAsync(appointment);

        return mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        await ValidatePatientExistsAsync(patientId);

        await EnsurePatientAppointmentQueryAccessAsync(patientId);

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByPatientIdAsync(patientId));
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        if (IsPatient())
        {
            throw new ForbiddenAccessException(
                "Patients are not allowed to search appointments by doctor.");
        }

        await ValidateDoctorExistsAsync(doctorId);

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByDoctorIdAsync(doctorId));
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status)
    {
        if (IsPatient())
        {
            var patient = await GetLoggedInPatientAsync();

            var appointments = await appointmentRepository.GetByPatientIdAsync(patient.PatientId);

            appointments = appointments
                .Where(a => a.Status == status.ToString())
                .ToList();

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByStatusAsync(status));
    }
    public async Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto)
    {
        if (dto is null)
        {
            throw new AppointmentRuleException("Appointment details are required.");
        }

        var patient = await GetLoggedInPatientAsync();

        var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);

        if (!doctor.IsActive)
        {
            throw new AppointmentRuleException("Doctor is inactive.");
        }

        if (dto.ScheduledDate.Date < DateTime.Today)
        {
            throw new AppointmentRuleException("Appointment date cannot be in the past.");
        }

        if (!TimeSlots.Slots.Contains(dto.TimeSlot))
        {
            throw new AppointmentRuleException("Invalid time slot selected.");
        }

        if (await appointmentRepository.IsSlotBookedAsync(
                dto.DoctorId,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
        {
            throw new ConflictException("This time slot is already booked.");
        }

        if (await appointmentRepository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                patient.PatientId,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
        {
            throw new ConflictException("Patient already has an appointment in this slot.");
        }

        if (await appointmentRepository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                patient.PatientId,
                dto.DoctorId,
                dto.ScheduledDate.Date))
        {
            throw new ConflictException(
                "Patient already has an active appointment with this doctor on the selected date.");
        }

        var appointment = mapper.Map<Appointment>(dto);

        appointment.PatientId = patient.PatientId;
        appointment.DoctorId = dto.DoctorId;
        appointment.ScheduledDate = dto.ScheduledDate.Date;
        appointment.TimeSlots = dto.TimeSlot;
        appointment.Status = AppointmentStatus.Pending.ToString();
        appointment.CancellationReason = null;
        appointment.CreatedDate = DateTime.Now;

        var savedAppointment = await appointmentRepository.AddAsync(appointment);

        var appointmentWithDetails = await appointmentRepository
            .GetByIdWithDetailsAsync(savedAppointment.AppointmentId);

        return mapper.Map<AppointmentDto>(appointmentWithDetails ?? savedAppointment);
    }

    private async Task<Patient> GetLoggedInPatientAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var patient = await patientRepository.GetByUserIdAsync(userId);

        if (patient is null)
        {
            throw new EntityNotFoundException("Patient", userId);
        }

        return patient;
    }

    public async Task<AppointmentDto> ChangeAppointmentStatusAsync(
    int appointmentId,
    UpdateAppointmentStatusDto dto)
    {
        ValidateAppointmentId(appointmentId);

        if (dto is null)
        {
            throw new AppointmentRuleException("Appointment status details are required.");
        }

        var appointment = await appointmentRepository.GetByIdWithDetailsAsync(appointmentId)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        await EnsureAppointmentAccessAsync(appointment);

        if (IsPatient() && dto.Status != AppointmentStatus.Cancelled)
        {
            throw new ForbiddenAccessException(
                "Patients are allowed only to cancel their own appointments.");
        }

        if (dto.Status == AppointmentStatus.Cancelled &&
            string.IsNullOrWhiteSpace(dto.CancellationReason))
        {
            throw new AppointmentRuleException("Cancellation reason is required.");
        }

        appointment.Status = dto.Status.ToString();

        appointment.CancellationReason = dto.Status == AppointmentStatus.Cancelled
            ? dto.CancellationReason?.Trim()
            : null;

        var updated = await appointmentRepository.UpdateAsync(appointmentId, appointment)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        var updatedWithDetails = await appointmentRepository
            .GetByIdWithDetailsAsync(updated.AppointmentId);

        return mapper.Map<AppointmentDto>(updatedWithDetails ?? updated);
    }


    public async Task<AppointmentDto> CancelAppointmentAsync(
        int appointmentId,
        string? reason)
    {
        return await ChangeAppointmentStatusAsync(
            appointmentId,
            new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Cancelled,
                CancellationReason = reason ?? "Cancelled by user"
            });
    }

    private ClaimsPrincipal? CurrentUser =>
    httpContextAccessor.HttpContext?.User;

    private bool IsPatient()
    {
        return CurrentUser?.IsInRole(Roles.Patient) == true;
    }

    private bool IsAdmin()
    {
        return CurrentUser?.IsInRole(Roles.Admin) == true;
    }

    private async Task EnsurePatientAppointmentQueryAccessAsync(int requestedPatientId)
    {
        if (IsAdmin())
        {
            return;
        }

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (loggedInPatient.PatientId != requestedPatientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's appointments.");
            }

            return;
        }
    }

    private async Task EnsureAppointmentAccessAsync(Appointment appointment)
    {
        if (IsAdmin())
        {
            return;
        }

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (appointment.PatientId != loggedInPatient.PatientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's appointment.");
            }

            return;
        }
    }

    private static void ValidateAppointmentId(int id)
    {
        if (id <= 0)
            throw new AppointmentRuleException(
                "Please provide a valid appointment reference.");
    }

    private async Task ValidatePatientExistsAsync(int id)
    {
        if (id <= 0)
            throw new AppointmentRuleException("Invalid patient.");

        if (await patientRepository.GetByIdAsync(id) is null)
            throw new EntityNotFoundException("Patient", id);
    }

    private async Task<Doctor> ValidateDoctorExistsAsync(int id)
    {
        if (id <= 0)
            throw new AppointmentRuleException("Invalid doctor.");

        return await doctorRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Doctor", id);
    }
}