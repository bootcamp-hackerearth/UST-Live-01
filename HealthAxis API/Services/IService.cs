namespace HealthAxis.API.Services
{
    public interface IService<TEntity, TReadDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        Task<List<TReadDto>> GetAllAsync(CancellationToken ct = default);

        Task<TReadDto?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<TReadDto> CreateAsync(TCreateDto createDto, CancellationToken ct = default);

        Task<TReadDto?> UpdateAsync(
            int id,
            TUpdateDto updateDto,
            CancellationToken ct = default);

        Task<TReadDto?> DeleteAsync(int id, CancellationToken ct = default);
    }
}
