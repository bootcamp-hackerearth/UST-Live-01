using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.Repositories.Implementations;
using HealthAxisHealth.API.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.UnitOfWork
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        private bool _disposed;

        #endregion

        #region Constructor

        public UnitOfWork(
            ApplicationDbContext context)
        {
            _context = context;

            Users = new UserRepository(context);
            Patients = new PatientRepository(context);
            Doctors = new DoctorRepository(context);
            Appointments = new AppointmentRepository(context);
            HealthRecords = new HealthRecordRepository(context);
        }

        #endregion

        #region Repositories

        public IUserRepository Users { get; }

        public IPatientRepository Patients { get; }

        public IDoctorRepository Doctors { get; }

        public IAppointmentRepository Appointments { get; }

        public IHealthRecordRepository HealthRecords { get; }

        #endregion

        #region Methods

        public async Task<int> CommitAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(
                cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}