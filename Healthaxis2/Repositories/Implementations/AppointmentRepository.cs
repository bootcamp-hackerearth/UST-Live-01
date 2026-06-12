using System;
using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Repositories.Interfaces;

namespace Healthaxis2.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext db;

        public AppointmentRepository(AppDbContext context)
        {
            db = context;
        }

        public List<Appointment> GetAll()
        {
            return db.Appointments.ToList();
        }

        public Appointment GetById(int id)
        {
            return db.Appointments.Find(id);
        }

        // 🔥 SLOT CHECK (VERY IMPORTANT)
        public bool IsSlotBooked(int doctorId, DateTime date, string slot)
        {
            return db.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.Slot == slot);
        }

        public void Add(Appointment appointment)
        {
            db.Appointments.Add(appointment);
            db.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            db.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var appt = db.Appointments.Find(id);
            db.Appointments.Remove(appt);
            db.SaveChanges();
        }
    }
}
