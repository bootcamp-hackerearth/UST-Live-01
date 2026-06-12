using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Healthaxis2.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext db;

        public AppointmentService(AppDbContext context)
        {
            db = context;
        }

        public List<Appointment> GetAll()
        {
            var appointments = db.Appointments.ToList();

            foreach (var appt in appointments)
            {
                // ✅ AUTO CANCEL LOGIC
                if (appt.ScheduledDate < DateTime.Today &&
                    appt.Status != "Completed" &&
                    appt.Status != "Cancelled")
                {
                    appt.Status = "Cancelled";
                    appt.CancellationReason = "Not Appeared";
                }
            }

            db.SaveChanges();

            return appointments;
        }

        public Appointment GetById(int id) => db.Appointments.Find(id);

        public string Create(Appointment model)
        {

            if (string.IsNullOrEmpty(model.Status))
                model.Status = "Pending";

            if (model.ScheduledDate <= DateTime.Today)
                throw new Exception("Appointment must be from tomorrow");

            var validSlots = new[]
            {
                "09:00 AM","10:00 AM","11:00 AM","02:00 PM","03:00 PM"
            };

            if (!validSlots.Contains(model.Slot))
                throw new Exception("Invalid slot selected");

            db.Appointments.Add(model);
            
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                throw new Exception(string.Join(", ", errors));
            }
            return "Appointment Added successfully";
        }

        public void UpdateStatus(int id, string status, string reason)
        {
            var appt = db.Appointments.Find(id);
            appt.Status = status;

            if (status == "Cancelled")
                appt.CancellationReason = reason;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var a = db.Appointments.Find(id);
            db.Appointments.Remove(a);
            db.SaveChanges();
        }
    }
}