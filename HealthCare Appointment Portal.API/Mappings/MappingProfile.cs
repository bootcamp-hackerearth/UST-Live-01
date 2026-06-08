using AutoMapper;
using System.Linq;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Mappings
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            // Patient Mapping

            CreateMap<Patient, PatientDto>()
                .ForMember(
                    dest => dest.AppointmentCount,
                    opt => opt.MapFrom(
                        src => src.Appointments.Count));

            CreateMap<Patient, PatientDto>()
                 .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.GetAge()));

            CreateMap<CreatePatientDto, Patient>();

            CreateMap<UpdatePatientDto, Patient>();


            // Doctor Mapping

            CreateMap<Doctor, DoctorDto>()
                .ForMember(
                    dest => dest.UpcomingAppointmentCount,
                    opt => opt.MapFrom(
                        src => src.Appointments.Count));

            CreateMap<CreateDoctorDto, Doctor>();

            CreateMap<UpdateDoctorDto, Doctor>();


            // Appointment Mapping

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(
                        src => src.Patient.FullName))
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(
                        src => src.Doctor.FullName))
               .ForMember(
                    dest => dest.HasHealthRecord,
                     opt => opt.MapFrom(src =>
                    src.HealthRecords.Any()));

            CreateMap<CreateAppointmentDto,
                Appointment>();

            CreateMap<UpdateAppointmentDto,
                Appointment>();


            // Health Record Mapping

            CreateMap<HealthRecord, HealthRecordDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(
                        src => src.Patient.FullName))
                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(
                        src => src.Doctor.FullName))
                .ForMember(
                    dest => dest.Specialisation,
                    opt => opt.MapFrom(
                        src => src.Doctor.Specialisation.ToString()));

            CreateMap<CreateHealthRecordDto,
                HealthRecord>();

            CreateMap<UpdateHealthRecordDto,
                HealthRecord>();

            // Insurance Mapping

            CreateMap<Insurance, InsuranceDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(
                        src => src.Patient.FullName))
                .ForMember(
                    dest => dest.IsExpired,
                    opt => opt.MapFrom(
                        src => src.IsExpired()))
                .ForMember(
                    dest => dest.DaysUntilExpiry,
                    opt => opt.MapFrom(
                        src => src.DaysUntilExpiry()));

            CreateMap<CreateInsuranceDto,
                Insurance>();

            CreateMap<UpdateInsuranceDto,
                Insurance>();
        }
    }
}