using System;
using System.Collections.Generic;
using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Services
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);

        Doctor GetDoctorById(int doctorId);

        List<Doctor> GetAllDoctors();

        List<Doctor> GetAllActiveDoctors();

        List<Doctor> SearchDoctorsBySpecialisation(Specialisation specialisation);

        List<Doctor> GetAvailableDoctorsBySpecialisation(
    Specialisation specialisation,
    DateTime appointmentDate,
    int slotNumber);

        Doctor UpdateDoctor(Doctor doctor);

        Doctor DeactivateDoctor(int doctorId);

        Doctor ReactivateDoctor(int doctorId);

        List<DateTime> GetOffDays(int doctorId);

        Doctor AddOffDay(int doctorId, DateTime offDay);

        Doctor RemoveOffDay(int doctorId, DateTime offDay);
    }
}