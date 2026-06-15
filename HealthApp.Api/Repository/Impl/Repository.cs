using HealthApp.Api.Repository.Interface;

namespace HealthApp.Api.Repository.Impl
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Task<T> addasync(T entity, CancellationToken cd = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> getallasync(CancellationToken cd = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> getbydob(DateOnly date, CancellationToken cd = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> getbyidasync(int id, CancellationToken cd = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> getbynameasync(string name, CancellationToken cd = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> updateasync(int id, T entity, CancellationToken cd = default)
        {
            throw new NotImplementedException();
        }
    }
}
