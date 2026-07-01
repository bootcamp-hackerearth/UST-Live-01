using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HealthApp.API.Service.Impl;

public class DoctorService(
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper) : IDoctorService
{
    public async Task<List<DoctorDto>> GetAllDoctorsAsync()
    {
        if (IsDoctor())
        {
            throw new ForbiddenAccessException(
                "Doctors are not allowed to search all doctors.");
        }

        return mapper.Map<List<DoctorDto>>(
            await doctorRepository.GetAllActiveAsync());
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(int doctorId)
    {
        ValidateDoctorId(doctorId);

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            if (loggedInDoctor.DoctorId != doctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's profile.");
            }
        }

        var doctor = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);

        if (!doctor.IsActive)
        {
            throw new EntityNotFoundException("Doctor", doctorId);
        }

        return mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> GetLoggedInDoctorProfileAsync()
    {
        var doctor = await GetLoggedInDoctorAsync();

        if (!doctor.IsActive)
        {
            throw new ForbiddenAccessException(
                "Your doctor profile is inactive.");
        }

        return mapper.Map<DoctorDto>(doctor);
    }

    public async Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(
        SpecialisationType specialisation)
    {
        if (IsDoctor())
        {
            throw new ForbiddenAccessException(
                "Doctors are not allowed to search doctors by specialisation.");
        }

        if (!Enum.IsDefined(typeof(SpecialisationType), specialisation))
        {
            throw new BusinessRuleException("Invalid specialisation.");
        }

        var doctors = await doctorRepository.GetActiveBySpecialisationAsync(specialisation);

        return mapper.Map<List<DoctorDto>>(doctors);
    }

    public async Task<DoctorAvailabilityDto> GetDoctorAvailabilityAsync(
        int doctorId,
        DateTime date)
    {
        ValidateDoctorId(doctorId);

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            if (loggedInDoctor.DoctorId != doctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's availability.");
            }
        }

        var doctor = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);

        if (!doctor.IsActive)
        {
            throw new BusinessRuleException("Doctor is inactive.");
        }

        if (date.Date < DateTime.Today)
        {
            throw new BusinessRuleException(
                "Cannot check availability for past dates.");
        }

        var appointments = await appointmentRepository.GetByDoctorIdAsync(doctorId);

        var booked = appointments
            .Where(a =>
                a.ScheduledDate.Date == date.Date &&
                a.Status != AppointmentStatus.Cancelled.ToString())
            .Select(a => a.TimeSlots)
            .ToList();

        return new DoctorAvailabilityDto
        {
            DoctorId = doctorId,
            Date = date.Date,
            AvailableSlots = TimeSlots.Slots
                .Where(slot => !booked.Contains(slot))
                .ToList()
        };
    }

    private async Task<Doctor> GetLoggedInDoctorAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var doctor = await doctorRepository.GetByUserIdAsync(userId);

        if (doctor is null)
        {
            throw new EntityNotFoundException("Doctor", userId);
        }

        return doctor;
    }

    private ClaimsPrincipal? CurrentUser =>
        httpContextAccessor.HttpContext?.User;

    private bool IsDoctor()
    {
        return CurrentUser?.IsInRole(Roles.Doctor) == true;
    }

    private static void ValidateDoctorId(int id)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "Please provide a valid doctor reference.");
        }
    }
}