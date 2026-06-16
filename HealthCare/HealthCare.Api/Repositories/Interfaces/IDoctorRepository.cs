using HealthCare.Api.Models;


namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        //Task<PagedResult<Doctor>> GetDoctorAsync(string specialisation = null, string searchTerm = null, bool orderByDesending = false, int pageNumber = 1, int pageSize = 10);

       // Task AddRangeAsync(List<AvailableSlots>slots);

        //Task <List<Doctor>>GetBySpecialisationAsync(string  specialisation);

        //Task<bool>  EmailExistAsync(string email);
    }
}
