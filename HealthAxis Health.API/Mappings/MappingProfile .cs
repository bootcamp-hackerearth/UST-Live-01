using AutoMapper;
using HealthAxisHealth.Shared.DTOs.AppointmentDtos;
using HealthAxisHealth.Shared.DTOs.DoctorDtos;
using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.Shared.DTOs.PatientDtos;
using HealthAxisHealth.Shared.DTOs.UserDtos;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Doctor
            CreateMap<Doctor, DoctorDto>();
            CreateMap<UpdateDoctorDto, Doctor>();

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
