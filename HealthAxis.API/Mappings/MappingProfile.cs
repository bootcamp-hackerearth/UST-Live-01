using AutoMapper;
using HealthAxis.API.DTO.AppointmentDtos;
using HealthAxis.API.DTO.DoctorDtos;
using HealthAxis.API.DTO.HealthRecordDtos;
using HealthAxis.API.DTO.PatientDtos;
using HealthAxis.API.Models;

namespace HealthAxis.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient
            CreateMap<Patient, PatientDto>().ReverseMap();

            // Doctor
            CreateMap<Doctor, DoctorDto>().ReverseMap();

            // Appointment
            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();

            // Health Record
            CreateMap<CreateHealthRecordDto, HealthRecord>();
            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        }
    }
}