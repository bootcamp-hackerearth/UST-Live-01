using AutoMapper;
using HealthAxis.API.Dtos.AppointmentDtos;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Impl;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();

        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
        {
            return null;
        }

        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        var appointment = _mapper.Map<Appointment>(dto);

        await _appointmentRepository.AddAsync(appointment);

        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var appointment = await _appointmentRepository.DeleteAsync(id);

        return appointment != null;
    }
}