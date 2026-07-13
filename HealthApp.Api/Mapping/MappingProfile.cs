using AutoMapper;
using HealthApp.Api.Models;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient mappings
            CreateMap<Patient, PatientDto>();


            CreateMap<PatientCreateDto, Patient>()
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.HealthRecords, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());


            CreateMap<RegisterPatientDto, Patient>()
                .IncludeBase<PatientCreateDto, Patient>();


            // Doctor mappings
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src => src.Specialisation.ToString()));


            CreateMap<DoctorCreateDto, Doctor>()
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(
                    dest => dest.IsActive,
                    opt => opt.MapFrom(src => true))
                .ForMember(
                    dest => dest.Appointments,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.HealthRecords,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.DoctorLeaves,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.User,
                opt => opt.Ignore());

            // Appointment mappings

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientId,
                    opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.DoctorId,
                    opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.FullName : string.Empty))
                .ForMember(dest => dest.ScheduledDate,
                    opt => opt.MapFrom(src => src.ScheduledDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<AppointmentCreateDto, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.ScheduledDate,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ScheduledDate)))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => AppointmentStatus.Pending))
                .ForMember(dest => dest.CancellationReason, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());

            // HealthRecord mappings
            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(dest => dest.PatientId,
                    opt => opt.MapFrom(src => src.PatientId ?? 0))
                .ForMember(dest => dest.DoctorId,
                    opt => opt.MapFrom(src => src.DoctorId ?? 0))
                .ForMember(dest => dest.AppointmentId,
                    opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        src.Patient != null ? src.Patient.FullName : string.Empty))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        src.Doctor != null ? src.Doctor.FullName : string.Empty))
                .ForMember(dest => dest.VisitDate,
                    opt => opt.MapFrom(src =>
                        src.VisitDate.ToDateTime(TimeOnly.MinValue)));

            CreateMap<HealthRecordCreateDto, HealthRecord>()
                .ForMember(dest => dest.RecordId, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Appointment, opt => opt.Ignore());

            CreateMap<DoctorLeave, DoctorLeaveDto>()
                .ForMember(
                    destination => destination.DoctorName,
                    options => options.MapFrom(source =>
                        source.Doctor != null
                            ? source.Doctor.FullName
                            : string.Empty));

            CreateMap<DoctorLeaveCreateDto, DoctorLeave>()
                .ForMember(
                    destination => destination.DoctorLeaveId,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.DoctorId,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Doctor,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.CreatedAtUtc,
                    options => options.Ignore());
        }
    }
}