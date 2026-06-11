using System;
using System.Collections.Generic;
using HealthAxis.Api.Models;

public interface IAppointmentRepository
{
    List<Appointment> GetAll();
    List<Appointment> GetByPatient(int patientId);
    List<Appointment> GetByDoctor(int doctorId);

    Appointment GetById(int id);

    bool IsSlotTaken(int doctorId, DateTime date, string slot);
    bool ExistsSameDay(int patientId, int doctorId, DateTime date);

    List<string> GetBookedSlots(int doctorId, DateTime date);

    void Add(Appointment appointment);
    void Update(Appointment appointment);

    void Save();
}