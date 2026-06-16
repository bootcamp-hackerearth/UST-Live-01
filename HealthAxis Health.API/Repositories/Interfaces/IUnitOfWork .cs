using HealthAxisHealth.API.Repositories.Interfaces;

namespace HealthAxisHealth.API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories

        IUserRepository Users { get; }

        IPatientRepository Patients { get; }

        IDoctorRepository Doctors { get; }

        IAppointmentRepository Appointments { get; }

        IHealthRecordRepository HealthRecords { get; }

        #endregion

        #region Methods

        Task<int> CommitAsync(
            CancellationToken cancellationToken = default);

        #endregion
    }
}