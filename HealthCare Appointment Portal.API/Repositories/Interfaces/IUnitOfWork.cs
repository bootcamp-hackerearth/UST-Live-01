using System;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IUnitOfWork
        : IDisposable
    {
        IPatientRepository Patients
        {
            get;
        }

        IDoctorRepository Doctors
        {
            get;
        }

        IAppointmentRepository Appointments
        {
            get;
        }

        IHealthRecordRepository HealthRecords
        {
            get;
        }

        IInsuranceRepository Insurances
        {
            get;
        }

        IUserRepository Users
        {
            get;
        }

        Task<int> CommitAsync();
    }
}