using AutoMapper;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class Service<TEntity, TReadDto, TCreateDto, TUpdateDto>
        : IService<TEntity, TReadDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        protected readonly IRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        public Service(
            IRepository<TEntity> repository,
            IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<List<TReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            List<TEntity> entities = await Repository.GetAllAsync(ct);

            return Mapper.Map<List<TReadDto>>(entities);
        }

        public async Task<TReadDto?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            TEntity? entity = await Repository.GetByIdAsync(id, ct);

            if (entity == null)
            {
                return default;
            }

            return Mapper.Map<TReadDto>(entity);
        }

        public async Task<TReadDto> CreateAsync(
            TCreateDto createDto,
            CancellationToken ct = default)
        {
            TEntity entity = Mapper.Map<TEntity>(createDto);

            TEntity createdEntity = await Repository.CreateAsync(entity, ct);

            return Mapper.Map<TReadDto>(createdEntity);
        }

        public async Task<TReadDto?> UpdateAsync(
            int id,
            TUpdateDto updateDto,
            CancellationToken ct = default)
        {
            TEntity entity = Mapper.Map<TEntity>(updateDto);

            TEntity? updatedEntity = await Repository.UpdateAsync(id, entity, ct);

            if (updatedEntity == null)
            {
                return default;
            }

            return Mapper.Map<TReadDto>(updatedEntity);
        }

        public async Task<TReadDto?> DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            TEntity? deletedEntity = await Repository.DeleteAsync(id, ct);

            if (deletedEntity == null)
            {
                return default;
            }

            return Mapper.Map<TReadDto>(deletedEntity);
        }
    }
}
