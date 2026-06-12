using HealthAxisApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisAppMVC.Services.Interfaces
{
    public interface IHealthRecordMvcService
    {
        IEnumerable<HealthRecordDto> GetByPatient(int patientId);

        bool Create(
            HealthRecordDto dto,
            out string error);
    }
}
