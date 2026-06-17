using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthAppDbContext context) : base(context)
        {
        }
    }
}
