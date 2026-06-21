using AutoMapper;
using HealthApp.API.Enums;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.PatientName))
            .ForMember(d => d.Gender, o => o.MapFrom(s => Enum.Parse<GenderType>(s.Gender)));

        CreateMap<CreatePatientDto, Patient>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.PatientId, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.Appointments, o => o.Ignore())
            .ForMember(d => d.HealthRecords, o => o.Ignore());

        CreateMap<UpdatePatientDto, Patient>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.PatientId, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.Appointments, o => o.Ignore())
            .ForMember(d => d.HealthRecords, o => o.Ignore());

        CreateMap<Doctor, DoctorDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.DoctorName))
            .ForMember(d => d.Specialisation, o => o.MapFrom(s => Enum.Parse<SpecialisationType>(s.Specialisation)));

        CreateMap<CreateDoctorDto, Doctor>()
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Specialisation, o => o.MapFrom(s => s.Specialisation.ToString()))
            .ForMember(d => d.DoctorId, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.YearsOfExperience, o => o.Ignore())
            .ForMember(d => d.IsActive, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.Appointments, o => o.Ignore())
            .ForMember(d => d.HealthRecords, o => o.Ignore());

        CreateMap<UpdateDoctorDto, Doctor>()
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Specialisation, o => o.MapFrom(s => s.Specialisation.ToString()))
            .ForMember(d => d.DoctorId, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.YearsOfExperience, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.Appointments, o => o.Ignore())
            .ForMember(d => d.HealthRecords, o => o.Ignore());

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => Enum.Parse<AppointmentStatus>(s.Status)))
            .ForMember(d => d.TimeSlot, o => o.MapFrom(s => s.TimeSlots))
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient != null ? s.Patient.PatientName : null))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor != null ? s.Doctor.DoctorName : null));

        CreateMap<BookAppointmentDto, Appointment>()
            .ForMember(d => d.TimeSlots, o => o.MapFrom(s => s.TimeSlot))
            .ForMember(d => d.AppointmentId, o => o.Ignore())
            .ForMember(d => d.Patient, o => o.Ignore())
            .ForMember(d => d.Doctor, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.CancellationReason, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.HealthRecord, o => o.Ignore());

        CreateMap<HealthRecord, HealthRecordDto>()
            .ForMember(d => d.PatientId, o => o.MapFrom(s => s.PatientId ?? 0))
            .ForMember(d => d.DoctorId, o => o.MapFrom(s => s.DoctorId ?? 0))
            .ForMember(d => d.AppointmentId, o => o.MapFrom(s => s.AppointmentId ?? 0))
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient != null ? s.Patient.PatientName : null))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor != null ? s.Doctor.DoctorName : null));

        CreateMap<AddHealthRecordDto, HealthRecord>()
            .ForMember(d => d.HealthRecordId, o => o.Ignore())
            .ForMember(d => d.Patient, o => o.Ignore())
            .ForMember(d => d.Doctor, o => o.Ignore())
            .ForMember(d => d.Appointment, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore());
    }
}