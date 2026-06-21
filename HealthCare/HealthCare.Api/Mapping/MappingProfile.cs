using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Appointments;
using HealthCare.Api.DTOs.HealthRecord;
using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.Models;
using AutoMapper;

namespace HealthCare.Api.Mapping
{
    public class MappingProfile : Profile
    {
       public MappingProfile() 
       {
        
            //patient
            CreateMap<PatientRegisterDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();
            CreateMap<Patient, PatientListDto>();   
            CreateMap<Patient, PatientListDto>();

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



        }

    }
}
