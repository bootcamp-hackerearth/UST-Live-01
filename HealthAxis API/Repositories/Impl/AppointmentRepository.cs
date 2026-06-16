using HealthAxis.API.Data;
using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthAxisDbContext context)
            : base(context)
        {
        }
    }
}
