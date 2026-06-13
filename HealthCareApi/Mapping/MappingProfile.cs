using AutoMapper;
using HealthCare.Shared.DTOs.Appointment;
using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Shared.DTOs.HealthRecord;
using HealthCare.Shared.DTOs.Patient;
using HealthCareApi;
using HealthCareWebApi;
//using HealthCareApi.Helper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity -> DTO
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.DoctorId,
                       opt => opt.MapFrom(src => src.DoctorId))
            .ForMember(dest => dest.Specialisation,
                       opt => opt.MapFrom(src => src.Specialisation));

        // DTO -> Entity
        CreateMap<DoctorDto, Doctor>()
            .ForMember(dest => dest.DoctorId,
                       opt => opt.MapFrom(src => src.DoctorId))
            .ForMember(dest => dest.Specialisation,
                       opt => opt.MapFrom(src => src.Specialisation));

        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        CreateMap<vw_PatientHealthHistory, HealthRecordDto>();
    }
}