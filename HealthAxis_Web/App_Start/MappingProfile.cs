using AutoMapper;
using HealthAxis.Shared.Dtos;    
using HealthAxis.Api.Models;
using System;

namespace HealthAxis.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.Specialisation,
        opt => opt.MapFrom(src =>
            (Specialisation)Enum.Parse(typeof(Specialisation), src.Specialisation)
        ));

            CreateMap<DoctorDto, Doctor>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src =>
                        src.Specialisation.ToString()
                    ));

            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src =>
                        src.Specialisation.ToString()
                    ));



            CreateMap<Patient, PatientDto>().ReverseMap();



            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src =>
                        (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), src.Status)
                    ));

            CreateMap<AppointmentDto, Appointment>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src =>
                        src.Status.ToString()
                    ));



            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        }
    }
}
