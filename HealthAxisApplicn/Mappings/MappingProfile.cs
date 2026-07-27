using AutoMapper;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatePatientDto, Patient>();  
            CreateMap<UpdatePatientDto, Patient>();   
            CreateMap<Patient, PatientDto>().ReverseMap();

            
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();


            CreateMap<Appointment, AppointmentDto>()
            .ForMember(
                dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.DoctorName)
            )
            
            .ForMember(
                    dest => dest.PatientName,   
                    opt => opt.MapFrom(src => src.Patient.PatientName)
                );


            CreateMap<AppointmentDto, Appointment>();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentStatusDto, Appointment>();


            CreateMap<HealthRecord, HealthRecordDto>()
            .ForMember(
                dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.DoctorName)
            );

            CreateMap<HealthRecordDto, HealthRecord>();
        }
    }
}
