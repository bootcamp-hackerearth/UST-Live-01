namespace HealthAxis.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<PagedResponse<DoctorDto>> GetAllAsync(
            PaginationParams paginationParams,
            CancellationToken ct = default);

        Task<DoctorDto?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<object> GetAvailabilityAsync(
            int id,
            CancellationToken ct = default);

        Task<DoctorDto> AddAsync(
            CreateDoctorDto dto,
            CancellationToken ct = default);

        Task<DoctorDto> UpdateAsync(
            int id,
            UpdateDoctorDto dto,
            CancellationToken ct = default);
    }
}