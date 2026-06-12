using AutoMapper;
using Healthaxis2.Models;
using Healthaxis2.Shared.DTOs;

namespace Healthaxis2.App_Start   // ✅ IMPORTANT namespace
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ✅ Patient
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();

            // ✅ Doctor
            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorDto, Doctor>();

            // ✅ Appointment
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient.PatientName))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor.DoctorName));

            CreateMap<AppointmentDto, Appointment>();

            // ✅ HealthRecord
            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor.DoctorName));

            CreateMap<HealthRecordDto, HealthRecord>();
        }
    }
}