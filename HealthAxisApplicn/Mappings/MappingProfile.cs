using AutoMapper;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Models.Dto;

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
