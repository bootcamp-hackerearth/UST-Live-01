using Healthaxis2.Models;
using System;
using System.Collections.Generic;

namespace Healthaxis2.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        Appointment GetById(int id);
        bool IsSlotBooked(int doctorId, DateTime date, string slot);
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int id);
    }
}