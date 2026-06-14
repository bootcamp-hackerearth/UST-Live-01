using AutoMapper;
using HealthAxisHealth.API.DTOs.AppointmentDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.DTOs.HealthRecordDtos;
using HealthAxisHealth.API.DTOs.PatientDtos;
using HealthAxisHealth.API.DTOs.UserDtos;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Doctor
            CreateMap<Doctor, DoctorDto>();

            // Patient
            CreateMap<Patient, PatientDto>()
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.GetAge()));

            // Appointment
            CreateMap<Appointment, AppointmentDto>();

            // Health Record
            CreateMap<HealthRecord, HealthRecordDto>();

            // User
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                    src.Doctor != null
                    ? src.Doctor.FullName
                    : src.Patient != null
                        ? src.Patient.FullName
                        : string.Empty));
        }
    }
}
