using AutoMapper;
using HealthAxisApplicn.Dto;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
        }
        
    }
}
