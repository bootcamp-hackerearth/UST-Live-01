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