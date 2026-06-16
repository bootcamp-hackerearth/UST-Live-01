using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;

namespace HealthApp.API.Repository.Impl
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthAppDbContext context) : base(context)
        {
        }
    }
}
