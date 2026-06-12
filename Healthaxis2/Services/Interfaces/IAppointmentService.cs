using System.Collections.Generic;
using Healthaxis2.Models;

namespace Healthaxis2.Services.Interfaces
{
    public interface IAppointmentService
    {
        List<Appointment> GetAll();
        Appointment GetById(int id);
        string Create(Appointment appointment); 
        void UpdateStatus(int id, string status, string reason);
        void Delete(int id);
    }
}