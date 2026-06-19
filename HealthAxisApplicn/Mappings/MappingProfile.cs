using AutoMapper;
using HealthAxisApplicn.Dto.Patients;
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
