using AutoMapper;
using HealthApp.API.Data;
using HealthApp.Shared.DTOs;
using System;

namespace HealthApp.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // PATIENT 
            CreateMap<Patient, PatientDto>();

            CreateMap<PatientDto, Patient>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate ?? DateTime.Now));


            // DOCTOR
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive));

            CreateMap<DoctorDto, Doctor>()
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive));


            // APPOINTMENT 
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        src.Patient != null ? src.Patient.FullName : null))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        src.Doctor != null ? src.Doctor.FullName : null));

            CreateMap<AppointmentDto, Appointment>()
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());


            // HEALTH RECORD
            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        src.Patient != null ? src.Patient.FullName : null))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        src.Doctor != null ? src.Doctor.FullName : null));

            CreateMap<HealthRecordDto, HealthRecord>()
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
        }
    }
}