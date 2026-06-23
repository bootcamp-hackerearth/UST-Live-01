using AutoMapper;
using HealthAxis.Shared.DTO.AppointmentDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.DTO.PatientDtos;
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