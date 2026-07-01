namespace HealthApp.Api.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>?> getallAsync(CancellationToken cd=default);

        Task<T?> getbyidAsync(int id, CancellationToken cd=default);

        Task<T> addAsync(T entity, CancellationToken cd = default);
        
        Task<T?> updateAsync(int id,T entity, CancellationToken cd = default);

        Task<T?> getbynameAsync(string name, CancellationToken cd = default);





        Task<(List<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize,
            CancellationToken cd = default);

    }
}
