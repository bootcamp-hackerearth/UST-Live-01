using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.Repositories.Implementations;
using HealthAxisHealth.API.Repositories.Interfaces;

namespace HealthAxisHealth.API.UnitOfWork
{
    public class UnitOfWork :
        IUnitOfWork
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor

        public UnitOfWork(
            ApplicationDbContext context)
        {
            _context = context;

            Users =
                new UserRepository(context);

            Patients =
                new PatientRepository(context);

            Doctors =
                new DoctorRepository(context);

            Appointments =
                new AppointmentRepository(context);

            HealthRecords =
                new HealthRecordRepository(context);
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

        public async Task<int> CommitAsync()
        {
            return await _context
                .SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}