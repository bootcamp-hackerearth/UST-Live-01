using Healthcare.Shared.DTOs.Patient;
using Healthcare.Shared.DTOs.Doctor;
using Healthcare.Shared.DTOs.Appointments;
using Healthcare.Shared.DTOs.HealthRecord;
using Healthcare.Shared.DTOs.Authentication;
using HealthCare.Api.Models;
using AutoMapper;

namespace HealthCare.Api.Mapping
{
    public class MappingProfile : Profile
    {
       public MappingProfile() 
       {
        
            //patient
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();
            CreateMap<Patient, PatientListDto>();   
            CreateMap<Patient, PatientListDto>();
            CreateMap<Patient, PatientListDto>()
                .ForMember(
                    dest => dest.HasInsurance,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.InsuranceId))
                );


            //Doctor
            CreateMap<DoctorRegisterDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();
            CreateMap<Doctor, DoctorListDto>();

            //Appointment
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();
            CreateMap<Appointment, AppointmentListDto>();


            //HealthRecord
            CreateMap<CreateHealthRecordDto, HealthRecord>();
            CreateMap<UpdateDoctorDto, HealthRecord>();
            CreateMap<HealthRecord, HealthRecordListDto>();
            CreateMap<HealthRecord, HealthRecordListDto>().ForMember(dest => dest.DoctorName,
               opt => opt.MapFrom(src => src.Doctor.FullName));



        }

    }
}
