using AutoMapper;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
          
            CreateMap<Patient, PatientResponseDto>();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();

           
            CreateMap<Doctor, DoctorResponseDto>();
            CreateMap<CreateDoctorDto, Doctor>();

          
            CreateMap<Appointment, AppointmentResponseDto>();
            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordResponseDto>();
            CreateMap<CreateHealthRecordDto, HealthRecord>();

            
        }
    }
}