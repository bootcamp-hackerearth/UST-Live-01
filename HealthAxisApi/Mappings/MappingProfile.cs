using AutoMapper;
using HealthAxisCore_Api.DTOs.Patient;
using HealthAxisCore_Api.DTOs.Doctor;
using HealthAxisCore_Api.DTOs.Appointment;
using HealthAxisCore_Api.DTOs.HealthRecord;
using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
          
            CreateMap<Patient, PatientResponseDTO>();
            CreateMap<CreatePatientDTO, Patient>();
            CreateMap<UpdatePatientDTO, Patient>();

           
            CreateMap<Doctor, DoctorResponseDTO>();
            CreateMap<CreateDoctorDTO, Doctor>();

          
            CreateMap<Appointment, AppointmentResponseDTO>();
            CreateMap<CreateAppointmentDTO, Appointment>();

            CreateMap<HealthRecord, HealthRecordResponseDTO>();
            CreateMap<CreateHealthRecordDTO, HealthRecord>();

            
        }
    }
}