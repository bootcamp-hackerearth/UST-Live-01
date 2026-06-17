using AutoMapper;
using HealthAxis.API.DTO;
using HealthAxis.API.Models;
using HealthAxis.DTO.AppointmentDto;
using HealthAxis.DTO.DoctorDto;
using HealthAxis.DTO.HealthRecordDto;

namespace HealthAxis.API.Mappings
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
