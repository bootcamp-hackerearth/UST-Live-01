using AutoMapper;
using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/healthrecord")]
    public class HealthRecordController : ApiController
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public HealthRecordController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet, Route("patient/{patientId}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var records = _context.HealthRecords
                .Where(h => h.PatientId == patientId)
                .OrderByDescending(h => h.VisitDate)
                .ToList();

            if (records == null || records.Count == 0)
                return Ok(new List<HealthRecordDto>());

            var result = records.Select(h => new HealthRecordDto
            {
                RecordId = h.RecordId,
                PatientId = h.PatientId,
                DoctorId = h.DoctorId,
                VisitDate = h.VisitDate,
                Diagnosis = h.Diagnosis,
                Prescription = h.Prescription,
                Notes = h.Notes,
                DoctorName = _context.Doctors
                                .Where(d => d.DoctorId == h.DoctorId)
                                .Select(d => d.FullName)
                                .FirstOrDefault(),
                Specialisation = _context.Doctors
                                .Where(d => d.DoctorId == h.DoctorId)
                                .Select(d => d.Specialisation)
                                .FirstOrDefault()
            }).ToList();

            return Ok(result);
        }
    }
}