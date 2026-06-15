using AutoMapper;
using HealthAxis.API.Models;
using HealthAxis.DTO.PatientDto;

namespace HealthAxis.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();

        }
    }
}
