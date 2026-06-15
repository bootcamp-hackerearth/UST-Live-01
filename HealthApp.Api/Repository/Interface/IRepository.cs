namespace HealthApp.Api.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> getallasync(CancellationToken cd=default);

        Task<T> getbyidasync(int id, CancellationToken cd=default);

        Task<T> addasync(T entity, CancellationToken cd = default);
        
        Task<T> updateasync(int id,T entity, CancellationToken cd = default);

        Task<T> getbydob(DateOnly date, CancellationToken cd = default);

        Task<T> getbynameasync(string name, CancellationToken cd = default);
          

    }
}
