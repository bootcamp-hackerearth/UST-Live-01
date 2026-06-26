using AutoMapper;
using HealthApp.Shared.Dto;
using HealthApp.Api.Model;

namespace HealthApp.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Doctor, DoctorDto>().ReverseMap();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        src.Patient != null ? src.Patient.FullName : string.Empty))

                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        src.Doctor != null ? src.Doctor.FullName : string.Empty));

            CreateMap<AppointmentDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        src.Patient != null ? src.Patient.FullName : string.Empty))

                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        src.Doctor != null ? src.Doctor.FullName : string.Empty));

            CreateMap<HealthRecordDto, HealthRecord>();


        }
    }
}