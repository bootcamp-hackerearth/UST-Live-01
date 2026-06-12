using Healthaxis2.Data;
using Healthaxis2.Models;
using System.Linq;
using System.Web.Http;

namespace Healthaxis2.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {
        private readonly AppDbContext db;

        // ✅ DI Constructor
        public PatientsController(AppDbContext context)
        {
            db = context;
        }

        // ✅ GET all patients
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(db.Patients.ToList());
        }

        // ✅ GET by ID
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var patient = db.Patients.Find(id);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        // ✅ CREATE
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Patients.Add(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        // ✅ UPDATE
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = db.Patients.Find(id);

            if (existing == null)
                return NotFound();

            // ✅ Update fields manually (IMPORTANT)
            existing.PatientName = patient.PatientName;
            existing.DateOfBirth = patient.DateOfBirth;
            existing.Gender = patient.Gender;
            existing.PhoneNumber = patient.PhoneNumber;
            existing.Email = patient.Email;
            existing.InsuranceId = patient.InsuranceId;

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(string.Join(", ", errors));
            }

            return Ok(existing);
        }

        // ✅ DELETE
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var patient = db.Patients.Find(id);
            if (patient == null) return NotFound();

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok("Patient deleted successfully");
        }
    }
}
