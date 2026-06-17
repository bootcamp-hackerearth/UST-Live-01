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
            // Patient
            CreateMap<Patient, PatientDto>().ReverseMap();

            // Doctor
            CreateMap<Doctor, DoctorDto>().ReverseMap();

            // Appointment
            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();

            // Health Record
            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        }
    }
}