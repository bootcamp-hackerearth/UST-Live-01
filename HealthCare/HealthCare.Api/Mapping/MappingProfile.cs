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
