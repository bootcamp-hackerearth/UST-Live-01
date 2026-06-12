using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

public class AppointmentRepositoryImpl : IAppointmentRepository
{
    private readonly AppDBContext _context;

    public AppointmentRepositoryImpl(AppDBContext context)
    {
        _context = context;
    }

    public List<Appointment> GetAll()
    {
        return _context.Appointments.ToList();
    }

    public List<Appointment> GetByPatient(int patientId)
    {
        return _context.Appointments
            .Include("Doctor")  
            .Where(a => a.PatientId == patientId)
            .ToList();
    }

    public List<Appointment> GetByDoctor(int doctorId)
    {
        return _context.Appointments
            .Include("Patient")
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.ScheduledDate)
            .ToList();
    }

    public Appointment GetById(int id)
    {
        return _context.Appointments.Find(id);
    }

    public bool ExistsSameDay(int patientId, int doctorId, DateTime date)
    {
        return _context.Appointments.Any(a =>
            a.PatientId == patientId &&
            a.DoctorId == doctorId &&
            DbFunctions.TruncateTime(a.ScheduledDate) ==
            DbFunctions.TruncateTime(date));
    }

    public bool IsSlotTaken(int doctorId, DateTime date, string slot)
    {
        return _context.Appointments.Any(a =>
            a.DoctorId == doctorId &&
            DbFunctions.TruncateTime(a.ScheduledDate) ==
            DbFunctions.TruncateTime(date) &&
            a.TimeSlot == slot);
    }

    public List<string> GetBookedSlots(int doctorId, DateTime date)
    {
        return _context.Appointments
            .Where(a => a.DoctorId == doctorId &&
                        DbFunctions.TruncateTime(a.ScheduledDate) ==
                        DbFunctions.TruncateTime(date))
            .Select(a => a.TimeSlot)
            .ToList();
    }

    public void Add(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
    }

    public void Update(Appointment appointment)
    {
        _context.Entry(appointment).State = EntityState.Modified;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}