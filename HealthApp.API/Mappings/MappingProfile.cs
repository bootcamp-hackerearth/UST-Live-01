using AutoMapper;
using HealthApp.API.Models;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

namespace HealthApp.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(
                destination => destination.FullName,
                options => options.MapFrom(
                    source => source.PatientName))
            .ForMember(
                destination => destination.Gender,
                options => options.MapFrom(
                    source => Enum.Parse<GenderType>(source.Gender)));

        CreateMap<CreatePatientDto, Patient>()
            .ForMember(
                destination => destination.PatientName,
                options => options.MapFrom(
                    source => source.FullName))
            .ForMember(
                destination => destination.Gender,
                options => options.MapFrom(
                    source => source.Gender.ToString()))
            .ForMember(
                destination => destination.PatientId,
                options => options.Ignore())
            .ForMember(
                destination => destination.UserId,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore())
            .ForMember(
                destination => destination.Appointments,
                options => options.Ignore())
            .ForMember(
                destination => destination.HealthRecords,
                options => options.Ignore());

        CreateMap<UpdatePatientDto, Patient>()
            .ForMember(
                destination => destination.PatientName,
                options => options.MapFrom(
                    source => source.FullName))
            .ForMember(
                destination => destination.Gender,
                options => options.MapFrom(
                    source => source.Gender.ToString()))
            .ForMember(
                destination => destination.PatientId,
                options => options.Ignore())
            .ForMember(
                destination => destination.UserId,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore())
            .ForMember(
                destination => destination.Appointments,
                options => options.Ignore())
            .ForMember(
                destination => destination.HealthRecords,
                options => options.Ignore());

        CreateMap<Doctor, DoctorDto>()
            .ForMember(
                destination => destination.FullName,
                options => options.MapFrom(
                    source => source.DoctorName))
            .ForMember(
                destination => destination.Specialisation,
                options => options.MapFrom(
                    source => Enum.Parse<SpecialisationType>(
                        source.Specialisation)));

        CreateMap<CreateDoctorDto, Doctor>()
            .ForMember(
                destination => destination.DoctorName,
                options => options.MapFrom(
                    source => source.FullName))
            .ForMember(
                destination => destination.Specialisation,
                options => options.MapFrom(
                    source => source.Specialisation.ToString()))
            .ForMember(
                destination => destination.DoctorId,
                options => options.Ignore())
            .ForMember(
                destination => destination.UserId,
                options => options.Ignore())
            .ForMember(
                destination => destination.YearsOfExperience,
                options => options.Ignore())
            .ForMember(
                destination => destination.IsActive,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore())
            .ForMember(
                destination => destination.Appointments,
                options => options.Ignore())
            .ForMember(
                destination => destination.HealthRecords,
                options => options.Ignore())
            .ForMember(
                destination => destination.DoctorLeaves,
                options => options.Ignore());

        CreateMap<UpdateDoctorDto, Doctor>()
            .ForMember(
                destination => destination.DoctorName,
                options => options.MapFrom(
                    source => source.FullName))
            .ForMember(
                destination => destination.Specialisation,
                options => options.MapFrom(
                    source => source.Specialisation.ToString()))
            .ForMember(
                destination => destination.DoctorId,
                options => options.Ignore())
            .ForMember(
                destination => destination.UserId,
                options => options.Ignore())
            .ForMember(
                destination => destination.YearsOfExperience,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore())
            .ForMember(
                destination => destination.Appointments,
                options => options.Ignore())
            .ForMember(
                destination => destination.HealthRecords,
                options => options.Ignore())
            .ForMember(
                destination => destination.DoctorLeaves,
                options => options.Ignore());

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(
                destination => destination.Status,
                options => options.MapFrom(
                    source => Enum.Parse<AppointmentStatus>(
                        source.Status)))
            .ForMember(
                destination => destination.TimeSlot,
                options => options.MapFrom(
                    source => source.TimeSlots))
            .ForMember(
                destination => destination.PatientName,
                options => options.MapFrom(
                    source => source.Patient != null
                        ? source.Patient.PatientName
                        : null))
            .ForMember(
                destination => destination.DoctorName,
                options => options.MapFrom(
                    source => source.Doctor != null
                        ? source.Doctor.DoctorName
                        : null));

        CreateMap<BookAppointmentDto, Appointment>()
            .ForMember(
                destination => destination.TimeSlots,
                options => options.MapFrom(
                    source => source.TimeSlot))
            .ForMember(
                destination => destination.AppointmentId,
                options => options.Ignore())
            .ForMember(
                destination => destination.PatientId,
                options => options.Ignore())
            .ForMember(
                destination => destination.Patient,
                options => options.Ignore())
            .ForMember(
                destination => destination.Doctor,
                options => options.Ignore())
            .ForMember(
                destination => destination.Status,
                options => options.Ignore())
            .ForMember(
                destination => destination.CancellationReason,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore())
            .ForMember(
                destination => destination.HealthRecord,
                options => options.Ignore());

        CreateMap<HealthRecord, HealthRecordDto>()
            .ForMember(
                destination => destination.PatientId,
                options => options.MapFrom(
                    source => source.PatientId ?? 0))
            .ForMember(
                destination => destination.DoctorId,
                options => options.MapFrom(
                    source => source.DoctorId ?? 0))
            .ForMember(
                destination => destination.AppointmentId,
                options => options.MapFrom(
                    source => source.AppointmentId ?? 0))
            .ForMember(
                destination => destination.PatientName,
                options => options.MapFrom(
                    source => source.Patient != null
                        ? source.Patient.PatientName
                        : null))
            .ForMember(
                destination => destination.DoctorName,
                options => options.MapFrom(
                    source => source.Doctor != null
                        ? source.Doctor.DoctorName
                        : null));

        CreateMap<AddHealthRecordDto, HealthRecord>()
            .ForMember(
                destination => destination.HealthRecordId,
                options => options.Ignore())
            .ForMember(
                destination => destination.Patient,
                options => options.Ignore())
            .ForMember(
                destination => destination.Doctor,
                options => options.Ignore())
            .ForMember(
                destination => destination.Appointment,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore());

        CreateMap<DoctorLeave, DoctorLeaveDto>()
            .ForMember(
                destination => destination.DoctorName,
                options => options.MapFrom(
                    source => source.Doctor != null
                        ? source.Doctor.DoctorName
                        : string.Empty));

        CreateMap<CreateDoctorLeaveDto, DoctorLeave>()
            .ForMember(
                destination => destination.DoctorLeaveId,
                options => options.Ignore())
            .ForMember(
                destination => destination.DoctorId,
                options => options.Ignore())
            .ForMember(
                destination => destination.Doctor,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedDate,
                options => options.Ignore());
    }
}