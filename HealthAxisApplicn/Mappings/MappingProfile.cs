using AutoMapper;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();

            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentStatusDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        }
        
    }
}
