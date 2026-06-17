using AutoMapper;
using HealthAxisHealth.Shared.DTOs.AppointmentDtos;
using HealthAxisHealth.Shared.DTOs.DoctorDtos;
using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.Shared.DTOs.PatientDtos;
using HealthAxisHealth.Shared.DTOs.UserDtos;
using HealthAxisHealth.API.Models;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Mappings
{
    [ExcludeFromCodeCoverage]
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
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => GetUserFullName(src)));
        }

        private static string GetUserFullName(User user)
        {
            if (user.Doctor != null)
            {
                return user.Doctor.FullName;
            }

            if (user.Patient != null)
            {
                return user.Patient.FullName;
            }

            return string.Empty;
        }
    }
}