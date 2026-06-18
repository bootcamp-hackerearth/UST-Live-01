using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Appointments;
using HealthCare.Api.DTOs.HealthRecord;
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
            CreateMap<PatientListDto, Patient>();
            //CreateMap<PatientDto, Patient>();

            //Doctor
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();
            CreateMap<DoctorListDto, Doctor>();
           // CreateMap<DoctorDto, Doctor>();

            //Appointment
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();
            CreateMap<AppointmentListDto, Appointment>();
            //CreateMap<AppointmentDto, Appointment>();

            //HealthRecord
            CreateMap<CreateHealthRecordDto, HealthRecord>();
            CreateMap<UpdateDoctorDto, HealthRecord>();
            CreateMap<HealthRecordListDto, HealthRecord>();
           

        }

    }
}
