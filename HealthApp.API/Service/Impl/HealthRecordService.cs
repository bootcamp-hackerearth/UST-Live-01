using AutoMapper;
using HealthApp.API.Enums;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;

namespace HealthApp.API.Service.Impl;

public class HealthRecordService(
    IHealthRecordRepository healthRecordRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    IMapper mapper) : IHealthRecordService
{
    public async Task<List<HealthRecordDto>> GetAllHealthRecordsAsync()
        => mapper.Map<List<HealthRecordDto>>(
            await healthRecordRepository.GetAllAsync());

    public async Task<HealthRecordDto> GetHealthRecordByIdAsync(int id)
    {
        if (id <= 0)
            throw new HealthRecordRuleException("Invalid health record.");

        var h = await healthRecordRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("HealthRecord", id);

        return mapper.Map<HealthRecordDto>(h);
    }

    public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(
        int patientId)
    {
        if (await patientRepository.GetByIdAsync(patientId) is null)
            throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<List<HealthRecordDto>>(
            await healthRecordRepository.GetByPatientIdAsync(patientId));
    }

    public async Task<HealthRecordDto> AddHealthRecordAsync(
        AddHealthRecordDto dto)
    {
        if (dto is null)
            throw new HealthRecordRuleException(
                "Health record details are required.");

        var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId)
            ?? throw new EntityNotFoundException(
                "Appointment",
                dto.AppointmentId);

        if (appointment.PatientId != dto.PatientId)
            throw new HealthRecordRuleException(
                "Appointment does not belong to selected patient.");

        if (appointment.Status == AppointmentStatus.Cancelled.ToString() ||
            appointment.Status == AppointmentStatus.Pending.ToString())
        {
            throw new HealthRecordRuleException(
                "Health record can be added only after consultation.");
        }

        if (await healthRecordRepository.ExistsByAppointmentIdAsync(dto.AppointmentId))
            throw new ConflictException(
                "Health record already exists for this appointment.");

        var h = mapper.Map<HealthRecord>(dto);
        h.PatientId = appointment.PatientId;
        h.DoctorId = appointment.DoctorId;
        h.AppointmentId = appointment.AppointmentId;
        h.CreatedDate = DateTime.Now;

        var saved = await healthRecordRepository.AddAsync(h);

        appointment.Status = AppointmentStatus.Completed.ToString();

        await appointmentRepository.UpdateAsync(
            appointment.AppointmentId,
            appointment);

        return mapper.Map<HealthRecordDto>(saved);
    }
}