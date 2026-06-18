using S3_HealthAxisApi.DTOs.Admin;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;

        public AdminService(
            IAdminRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdminDashboardDto>
            GetDashboardAsync()
        {
            return new AdminDashboardDto
            {
                TotalPatients =
                    await _repository.CountPatientsAsync(),

                ActivePatients =
                    await _repository.CountActivePatientsAsync(),

                TotalDoctors =
                    await _repository.CountDoctorsAsync(),

                ActiveDoctors =
                    await _repository.CountActiveDoctorsAsync(),

                TodayAppointments =
                    await _repository.CountTodayAppointmentsAsync(),

                PendingAppointments =
                    await _repository.CountPendingAppointmentsAsync(),

                CompletedAppointments =
                    await _repository.CountCompletedAppointmentsAsync()
            };
        }

        public async Task<AdminStatisticsDto>
            GetStatisticsAsync()
        {
            return new AdminStatisticsDto
            {
                Patients =
                    await _repository.CountPatientsAsync(),

                Doctors =
                    await _repository.CountDoctorsAsync(),

                Appointments =
                    await _repository.CountTodayAppointmentsAsync(),

                HealthRecords =
                    await _repository.CountHealthRecordsAsync()
            };
        }

        public async Task<IEnumerable<UserManagementDto>>
            GetUsersAsync()
        {
            var users =
                await _repository.GetUsersAsync();

            return users.Select(u =>
                new UserManagementDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    ReferenceId = u.ReferenceId
                });
        }

        public async Task<UserManagementDto?>
            GetUserByIdAsync(int id)
        {
            var user =
                await _repository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            return new UserManagementDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role.ToString(),
                ReferenceId = user.ReferenceId
            };
        }
    }
}