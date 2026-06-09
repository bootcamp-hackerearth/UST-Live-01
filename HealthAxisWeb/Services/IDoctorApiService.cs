using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisWeb.Services
{
    public interface IDoctorApiService
    {
        Task<List<DoctorDto>> GetAllAsync(); //it return smthg that happens in future, to avoid hang. difference between synchronous and asynchronous
        Task<DoctorDto> GetByIdAsync(int id);
        Task<DoctorDto> AddAsync(DoctorDto doctorDto);
        Task<DoctorDto> UpdateAsync(int id,DoctorDto doctorDto);
    }
}
