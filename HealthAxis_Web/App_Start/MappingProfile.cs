using AutoMapper;
using HealthAxis_Web.Models;
using HealthAxis.Shared.Dtos;
using System;

namespace HealthAxis_Web.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src =>
                        (DoctorDto.SpecialisationType)Enum.Parse(
                            typeof(DoctorDto.SpecialisationType),
                            src.Specialisation.Trim(),
                            true)));

            CreateMap<DoctorDto, Doctor>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src => src.Specialisation.ToString()));

            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();

        }
    }
}
