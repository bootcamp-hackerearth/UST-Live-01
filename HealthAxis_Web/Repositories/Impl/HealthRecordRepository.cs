using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAxis.Api.Database;
using System.Linq;
using System.Data.Entity;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;


namespace HealthAxis.Api.Repositories
{ 
  public class HealthRecordRepositoryImpl : IHealthRecordRepository
  {
      private readonly AppDBContext _context;
  
      public HealthRecordRepositoryImpl(AppDBContext context)
      {
          _context = context;
      }
  
      public List<HealthRecord> GetByPatientId(int patientId)
      {
          return _context.HealthRecords
              .Where(h => h.PatientId == patientId)
              .ToList();
      }
  }
}