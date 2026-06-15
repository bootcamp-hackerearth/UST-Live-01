using AutoMapper;
using HealthAxis.API.Models;
using HealthAxis.API.Dtos.DoctorDtos;
using HealthAxis.API.Dtos.PatientDtos;
using HealthAxis.API.Dtos.AppointmentDtos;
using HealthAxis.API.Dtos.HealthRecordDtos;

namespace HealthAxis.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDto>();

            CreateMap<CreateDoctorDto, Doctor>();

            CreateMap<UpdateDoctorDto, Doctor>();


            CreateMap<Patient, PatientDto>();

            CreateMap<UpdatePatientDto, Patient>();


            CreateMap<Appointment, AppointmentDto>();

            CreateMap<CreateAppointmentDto, Appointment>();


            CreateMap<HealthRecord, HealthRecordDto>();

            CreateMap<CreateHealthRecordDto, HealthRecord>();
        }
    }
}