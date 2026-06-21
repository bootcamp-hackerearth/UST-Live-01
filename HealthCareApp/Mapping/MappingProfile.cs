using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Models;
using HealthCareApp.Models.Dtos;
using SharedClasses.Dtos;

namespace HealthCareApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient mappings
            CreateMap<Patient, PatientDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => src.PatientName)
                )
                .ForMember(
                    dest => dest.InsuranceId,
                    opt => opt.MapFrom(src => src.InsuranceID)
                )
                .ForMember(
                    dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd"))
                )
                .ForMember(
                    dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd"))
                );

            CreateMap<CreatePatientDto, Patient>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.FullName)
                )
                .ForMember(
                    dest => dest.InsuranceID,
                    opt => opt.MapFrom(src => src.InsuranceId)
                );

            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.FullName)
                )
                .ForMember(
                    dest => dest.InsuranceID,
                    opt => opt.MapFrom(src => src.InsuranceId)
                );

            CreateMap<PatientRegisterDto, Patient>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.FullName)
                )
                .ForMember(
                    dest => dest.InsuranceID,
                    opt => opt.MapFrom(src => src.InsuranceId)
                );

            // Doctor mappings
            CreateMap<Doctor, DoctorDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => src.DoctorName)
                );

            CreateMap<CreateDoctorDto, Doctor>()
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.FullName)
                );

            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.FullName)
                );

            // Appointment mappings
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null)
                )
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null)
                )
                .ForMember(
                    dest => dest.ScheduledDate,
                    opt => opt.MapFrom(src => src.ScheduledDate.ToString("yyyy-MM-dd"))
                );

            CreateMap<BookAppointmentDto, Appointment>();

            CreateMap<UpdateAppointmentDto, Appointment>();

            // HealthRecord mappings
            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null)
                )
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null)
                )
                .ForMember(
                    dest => dest.VisitDate,
                    opt => opt.MapFrom(src => src.VisitDate.ToString("yyyy-MM-dd"))
                );

            CreateMap<AddHealthRecordDto, HealthRecord>();

            CreateMap<UpdateHealthRecordDto, HealthRecord>();
        }
    }
}