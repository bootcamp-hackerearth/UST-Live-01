using AutoMapper;
using HealthAxisCore_Api.DTOs.Patient;
using HealthAxisCore_Api.DTOs.Doctor;
using HealthAxisCore_Api.DTOs.Appointment;
using HealthAxisCore_Api.DTOs.HealthRecord;
using HealthAxisCore_Api.DTOs.User;
using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //  Patient
            CreateMap<Patient, PatientResponseDTO>();
            CreateMap<CreatePatientDTO, Patient>();
            CreateMap<UpdatePatientDTO, Patient>();

            //  Doctor
            CreateMap<Doctor, DoctorResponseDTO>();
            CreateMap<CreateDoctorDTO, Doctor>();

            //  Appointment
            CreateMap<Appointment, AppointmentResponseDTO>();
            CreateMap<CreateAppointmentDTO, Appointment>();

            //  HealthRecord
            CreateMap<HealthRecord, HealthRecordResponseDTO>();
            CreateMap<CreateHealthRecordDTO, HealthRecord>();

            //  User
            CreateMap<User, UserResponseDTO>();
            CreateMap<CreateUserDTO, User>();
        }
    }
}