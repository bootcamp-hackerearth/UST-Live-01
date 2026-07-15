using AutoMapper;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;

namespace HealthAxisCore_Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<UpdatePatientDto, Patient>();
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<CreateDoctorDto, Doctor>().ForMember(d => d.DoctorId, o => o.Ignore()).ForMember(d => d.IsActive, o => o.MapFrom(_ => true));
            CreateMap<UpdateDoctorDto, Doctor>();
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.PatientName))
                .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor.DoctorName))
                .ForMember(d => d.Specialisation, o => o.MapFrom(s => s.Doctor.Specialisation));
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(d => d.AppointmentId, o => o.Ignore())
                .ForMember(d => d.PatientId, o => o.Ignore())
                .ForMember(d => d.Patient, o => o.Ignore())
                .ForMember(d => d.Doctor, o => o.Ignore())
                .ForMember(d => d.Status, o => o.MapFrom(_ => "Pending"))
                .ForMember(d => d.CancellationReason, o => o.MapFrom(_ => string.Empty));
            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.PatientName))
                .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor.DoctorName));
            CreateMap<CreateHealthRecordDto, HealthRecord>()
                .ForMember(d => d.HealthRecordId, o => o.Ignore())
                .ForMember(d => d.DoctorId, o => o.Ignore())
                .ForMember(d => d.Patient, o => o.Ignore())
                .ForMember(d => d.Doctor, o => o.Ignore())
                .ForMember(d => d.Appointment, o => o.Ignore())
                .ForMember(d => d.VisitDate, o => o.MapFrom(_ => DateTime.UtcNow));
        }
    }
}