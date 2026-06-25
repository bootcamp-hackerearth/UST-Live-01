using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Repository.Interface
{
    public interface IAdminRepository
    {
        Task<int> CountPatientsAsync();

        Task<int> CountActivePatientsAsync();

        Task<int> CountDoctorsAsync();

        Task<int> CountActiveDoctorsAsync();

        Task<int> CountTodayAppointmentsAsync();

        Task<int> CountPendingAppointmentsAsync();

        Task<int> CountCompletedAppointmentsAsync();

        Task<int> CountHealthRecordsAsync();

        Task<IEnumerable<User>> GetUsersAsync();

        Task<bool> ResolveUserActiveStatusAsync(string email, string role);

        Task<User?> GetUserByIdAsync(int id);
    }
}