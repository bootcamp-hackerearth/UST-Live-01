using AutoMapper;
using HealthApp.API.Constants;
using HealthApp.API.Enums;
using HealthApp.API.Exceptions;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;

namespace HealthApp.API.Service.Impl;

public class DoctorService(
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    IMapper mapper) : IDoctorService
{
    public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        => mapper.Map<List<DoctorDto>>(
            await doctorRepository.GetAllActiveAsync());

    public async Task<DoctorDto> GetDoctorByIdAsync(int doctorId)
    {
        ValidateDoctorId(doctorId);

        var d = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);

        if (!d.IsActive)
            throw new EntityNotFoundException("Doctor", doctorId);

        return mapper.Map<DoctorDto>(d);
    }

    public async Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(
        SpecialisationType specialisation)
        => mapper.Map<List<DoctorDto>>(
            await doctorRepository.GetActiveBySpecialisationAsync(specialisation));

    public async Task<DoctorAvailabilityDto> GetDoctorAvailabilityAsync(
        int doctorId,
        DateTime date)
    {
        ValidateDoctorId(doctorId);

        var d = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);

        if (!d.IsActive)
            throw new BusinessRuleException("Doctor is inactive.");

        if (date.Date < DateTime.Today)
            throw new BusinessRuleException(
                "Cannot check availability for past dates.");

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
                .Where(s => !booked.Contains(s))
                .ToList()
        };
    }

    private static void ValidateDoctorId(int id)
    {
        if (id <= 0)
            throw new BusinessRuleException(
                "Please provide a valid doctor reference.");
    }
}