using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HospitalManagementAPI.Model;

namespace HealthApp.Api.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();

            CreateMap<Doctor, DoctorDto>().ReverseMap();

            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();


        }
    }
}
