using System.Collections.Generic;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;

namespace Healthaxis2.Web.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAll();
        Task<DoctorDto> GetById(int id);
        Task<bool> Create(DoctorDto dto);
        Task<bool> Update(int id, DoctorDto dto);
        Task<bool> Delete(int id);
    }
}
