using AutoMapper;
using HealthcareApi.Dtos;
using HealthcareApi.Models;

namespace HealthcareApi.App_Start
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

            CreateMap<Appointment, AppointmentDto>();
            CreateMap<BookAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordDto>();
            CreateMap<AddHealthRecordDto, HealthRecord>();
            CreateMap<UpdateHealthRecordDto, HealthRecord>();
        }
    }
}