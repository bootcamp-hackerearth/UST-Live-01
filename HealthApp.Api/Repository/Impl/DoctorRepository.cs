using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HospitalManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly HealthAppDbContext _context;
        public DoctorRepository(HealthAppDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task<Doctor?> searchbyspecialisationAsync(string specialisation)
        {
            var exiting = await _context.Set<Doctor>().FirstOrDefaultAsync
                    (d => d.Specialisation == specialisation);
            if (exiting == null) return null;
            return exiting;
        }
    }
}
