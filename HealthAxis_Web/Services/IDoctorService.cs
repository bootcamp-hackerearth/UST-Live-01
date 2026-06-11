using HealthAxis.Shared;
using System.Collections.Generic;
using HealthAxis.Shared.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IDoctorService
    {
        List<DoctorDto> GetAll(string specialisation);

        DoctorDto GetById(int id);

        void Add(CreateDoctorDto dto);

        void Update(int id, UpdateDoctorDto dto);
    }
}