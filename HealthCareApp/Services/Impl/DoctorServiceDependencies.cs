using AutoMapper;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace HealthCareApp.Services
{
    public sealed class DoctorServiceDependencies
    {
        public required IDoctorRepository Repository { get; init; }

        public required IAppointmentRepository AppointmentRepository { get; init; }

        public required IMapper Mapper { get; init; }

        public required UserManager<IdentityUser> UserManager { get; init; }

        public required RoleManager<IdentityRole> RoleManager { get; init; }

        public required ICacheService CacheService { get; init; }

        public required IDoctorLeaveService DoctorLeaveService { get; init; }

        public required ILogger<DoctorService> Logger { get; init; }
    }
}