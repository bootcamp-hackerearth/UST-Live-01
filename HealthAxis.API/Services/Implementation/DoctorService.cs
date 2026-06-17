using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.DTO.DoctorDto;

namespace HealthAxis.API.Services.Implementation
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor =await repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> GetAvailabilityAsync(int id)
        {
            var doctor =await repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }
    }
}