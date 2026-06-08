using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class UnitOfWork
        : IUnitOfWork
    {
        private readonly
            ApplicationDbContext
            _context;

        public UnitOfWork(
            ApplicationDbContext context)
        {
            _context =
                context;

            Patients =
                new PatientRepository(
                    _context);

            Doctors =
                new DoctorRepository(
                    _context);

            Appointments =
                new AppointmentRepository(
                    _context);

            HealthRecords =
                new HealthRecordRepository(
                    _context);

            Insurances =
                new InsuranceRepository(
                    _context);

            Users =
                new UserRepository(
                    _context);
        }

        public IPatientRepository
            Patients
        {
            get;
            private set;
        }

        public IDoctorRepository
            Doctors
        {
            get;
            private set;
        }

        public IAppointmentRepository
            Appointments
        {
            get;
            private set;
        }

        public IHealthRecordRepository
            HealthRecords
        {
            get;
            private set;
        }

        public IInsuranceRepository
            Insurances
        {
            get;
            private set;
        }

        public IUserRepository
            Users
        {
            get;
            private set;
        }

        public async Task<int>
            CommitAsync()
        {
            try
            {
                return await _context
                    .SaveChangesAsync();
            }
            catch (
                DbEntityValidationException ex)
            {
                StringBuilder errors =
                    new StringBuilder();

                foreach (
                    var entityErrors
                    in ex.EntityValidationErrors)
                {
                    foreach (
                        var validationError
                        in entityErrors.ValidationErrors)
                    {
                        errors.Append(
                            $"{validationError.PropertyName}: " +
                            $"{validationError.ErrorMessage}");
                    }
                }

                throw new ValidationException(
                    errors.ToString());
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}