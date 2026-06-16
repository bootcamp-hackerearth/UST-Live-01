using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;


namespace HealthApp.API.Repository.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HealthAppDbContext context) : base(context)
        {

        }
    }
}