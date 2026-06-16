using AutoMapper;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        }

    }
}
