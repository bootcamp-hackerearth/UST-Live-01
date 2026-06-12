using System.Collections.Generic;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;

namespace Healthaxis2.Web.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAll();
        Task<bool> Create(AppointmentDto dto);
        Task<bool> UpdateStatus(int id, string status, string reason);
        Task<bool> Delete(int id);
    }
}