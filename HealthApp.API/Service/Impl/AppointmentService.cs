using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

namespace HealthApp.API.Service.Impl;

public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IMapper mapper) : IAppointmentService
{
    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        => mapper.Map<List<AppointmentDto>>(await appointmentRepository.GetAllAsync());

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
    {
        ValidateAppointmentId(appointmentId);

        var appointment = await appointmentRepository.GetByIdWithDetailsAsync(appointmentId)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        return mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        await ValidatePatientExistsAsync(patientId);

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByPatientIdAsync(patientId));
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        await ValidateDoctorExistsAsync(doctorId);

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByDoctorIdAsync(doctorId));
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        => mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByStatusAsync(status));

    public async Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto)
    {
        if (dto is null)
            throw new AppointmentRuleException("Appointment details are required.");

        await ValidatePatientExistsAsync(dto.PatientId);

        var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);

        if (!doctor.IsActive)
            throw new AppointmentRuleException("Doctor is inactive.");

        if (dto.ScheduledDate.Date < DateTime.Today)
            throw new AppointmentRuleException("Appointment date cannot be in the past.");

        if (!TimeSlots.Slots.Contains(dto.TimeSlot))
            throw new AppointmentRuleException("Invalid time slot selected.");

        if (await appointmentRepository.IsSlotBookedAsync(
                dto.DoctorId,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
            throw new ConflictException("This time slot is already booked.");

        if (await appointmentRepository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                dto.PatientId,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
            throw new ConflictException("Patient already has an appointment in this slot.");

        var a = mapper.Map<Appointment>(dto);
        a.ScheduledDate = dto.ScheduledDate.Date;
        a.Status = AppointmentStatus.Pending.ToString();
        a.CancellationReason = null;
        a.CreatedDate = DateTime.Now;

        return mapper.Map<AppointmentDto>(
            await appointmentRepository.AddAsync(a));
    }

    public async Task<AppointmentDto> ChangeAppointmentStatusAsync(
        int appointmentId,
        UpdateAppointmentStatusDto dto)
    {
        ValidateAppointmentId(appointmentId);

        var a = await appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        if (dto.Status == AppointmentStatus.Cancelled &&
            string.IsNullOrWhiteSpace(dto.CancellationReason))
        {
            throw new AppointmentRuleException("Cancellation reason is required.");
        }

        a.Status = dto.Status.ToString();
        a.CancellationReason = dto.Status == AppointmentStatus.Cancelled
            ? dto.CancellationReason?.Trim()
            : null;

        var updated = await appointmentRepository.UpdateAsync(appointmentId, a)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        return mapper.Map<AppointmentDto>(updated);
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