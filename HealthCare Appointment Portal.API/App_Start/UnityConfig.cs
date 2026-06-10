using AutoMapper;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Mappings;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Services;
using System;
using Unity;

namespace HealthCare_Appointment_Portal
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
            new Lazy<IUnityContainer>(() =>
            {
                var container =
                    new UnityContainer();

                RegisterTypes(container);

                return container;
            });

        public static IUnityContainer Container
        {
            get
            {
                return container.Value;
            }
        }

        public static void RegisterTypes(
            IUnityContainer container)
        {
            var mapperConfiguration =
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<
                        MappingProfile>();
                });

            IMapper mapper =
                mapperConfiguration
                    .CreateMapper();

            container.RegisterInstance<
                IMapper>(mapper);

            container.RegisterType<
                ApplicationDbContext,
                ApplicationDbContext>();


            // Patient

            container.RegisterType<
                IPatientRepository,
                PatientRepository>();

            container.RegisterType<
                IPatientService,
                PatientService>();


            // Doctor

            container.RegisterType<
                IDoctorRepository,
                DoctorRepository>();

            container.RegisterType<
                IDoctorService,
                DoctorService>();


            // Appointment

            container.RegisterType<
                IAppointmentRepository,
                AppointmentRepository>();

            container.RegisterType<
                IAppointmentService,
                AppointmentService>();


            // Health Record

            container.RegisterType<
                IHealthRecordRepository,
                HealthRecordRepository>();

            container.RegisterType<
                IHealthRecordService,
                HealthRecordService>();

            // Insurance

            container.RegisterType<
                IInsuranceRepository,
                InsuranceRepository>();

            container.RegisterType<
                IInsuranceService,
                InsuranceService>();

            container.RegisterType<
                IUnitOfWork,
                UnitOfWork>();

            // User
            container.RegisterType<
                IUserRepository,
                UserRepository>();

            container.RegisterType<
                IUserService,
                UserService>();


        }
    }
}