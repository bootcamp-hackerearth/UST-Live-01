using AutoMapper;
using HealthAxis.API.Dtos.DoctorDtos;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Impl;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<List<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return _mapper.Map<List<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
            return null;

        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto)
    {
        var doctor = _mapper.Map<Doctor>(dto);

        await _doctorRepository.AddAsync(doctor);

        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto dto)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
            return null;

        _mapper.Map(dto, doctor);

        await _doctorRepository.UpdateAsync(doctor);

        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<bool> DeleteDoctorAsync(int id)
    {
        var doctor = await _doctorRepository.DeleteAsync(id);

        return doctor != null;
    }
}