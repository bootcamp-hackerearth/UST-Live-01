using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.DTOs.Users;
using HealthAxis.API.Models;

namespace HealthAxis.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateUserMaps();
            CreatePatientMaps();
            CreateDoctorMaps();
            CreateAppointmentMaps();
            CreateHealthRecordMaps();
        }

        private void CreateUserMaps()
        {
            CreateMap<User, UserReadDto>();

            CreateMap<UserCreateDto, User>();

            CreateMap<UserUpdateDto, User>();
        }

        private void CreatePatientMaps()
        {
            CreateMap<Patient, PatientReadDto>()
                .ForMember(
                    destination => destination.Age,
                    option => option.MapFrom(source => source.GetAge()));

            CreateMap<PatientCreateDto, Patient>()
                .ForMember(
                    destination => destination.CreatedDate,
                    option => option.MapFrom(_ => DateTime.Now));

            CreateMap<PatientUpdateDto, Patient>();
        }

        private void CreateDoctorMaps()
        {
            CreateMap<Doctor, DoctorReadDto>();

            CreateMap<DoctorCreateDto, Doctor>();

            CreateMap<DoctorUpdateDto, Doctor>();
        }

        private void CreateAppointmentMaps()
        {
            CreateMap<Appointment, AppointmentReadDto>();

            CreateMap<AppointmentCreateDto, Appointment>();

            CreateMap<AppointmentUpdateDto, Appointment>();
        }

        private void CreateHealthRecordMaps()
        {
            CreateMap<HealthRecord, HealthRecordReadDto>();

            CreateMap<HealthRecordCreateDto, HealthRecord>();

            CreateMap<HealthRecordUpdateDto, HealthRecord>();
        }

    }
}

