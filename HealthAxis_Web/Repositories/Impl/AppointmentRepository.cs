using HealthAxis.Api.Models;
using System;
using HealthAxis.Api.Database;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class AppointmentRepositoryImpl : IAppointmentRepository
    {
        private readonly AppDBContext _context;

        public AppointmentRepositoryImpl(AppDBContext context)
        {
            _context = context;
        }

        public List<Appointment> GetByDoctor(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToList();
        }

        public List<Appointment> GetByPatient(int patientId)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToList();
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments.Find(id);
        }

        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }

        public void Update(Appointment appointment)
        {
            _context.Entry(appointment).State =
                System.Data.Entity.EntityState.Modified;
        }

        public bool ExistsSameDay(int patientId, int doctorId, DateTime date)
        {
            return _context.Appointments.Any(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == date);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}