using SharedClasses.Enums;
using HealthcareApi.Models;
using System.Collections.Generic;

namespace HealthcareApi.Repositories
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();

        List<Doctor> GetAllActive();

        Doctor GetById(int doctorId);

        List<Doctor> GetBySpecialisation(Specialisation specialisation);

        List<Doctor> GetActiveBySpecialisation(Specialisation specialisation);

        Doctor Add(Doctor doctor);

        Doctor Update(int doctorId, Doctor doctor);
    }
}