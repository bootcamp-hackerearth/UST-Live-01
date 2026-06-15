using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Impl;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(HealthAxisDbContext context) : base(context)
    {
    }

    public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Appointments
            .Where(x => x.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
    {
        return await _context.Appointments
            .Where(x => x.DoctorId == doctorId)
            .ToListAsync();
    }
}