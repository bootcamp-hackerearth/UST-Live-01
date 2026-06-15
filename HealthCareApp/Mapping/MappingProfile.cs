using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Models;
using SharedClasses.Dtos;

namespace HealthCareApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>();

            CreateMap<CreatePatientDto, Patient>();

            CreateMap<UpdatePatientDto, Patient>();

            CreateMap<Doctor, DoctorDto>();

            CreateMap<CreateDoctorDto, Doctor>();

            CreateMap<UpdateDoctorDto, Doctor>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null)
                )
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null)
                );

            CreateMap<BookAppointmentDto, Appointment>();

            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordDto>()
    .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : null))
    .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null));

            CreateMap<AddHealthRecordDto, HealthRecord>();

            CreateMap<UpdateHealthRecordDto, HealthRecord>();
        }
    }
}