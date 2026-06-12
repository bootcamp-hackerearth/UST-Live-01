using System.Linq;
using System.Web.Http;
using Healthaxis2.Data;
using Healthaxis2.Models;

namespace Healthaxis2.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorsController : ApiController
    {
        private readonly AppDbContext db;

        public DoctorsController(AppDbContext context)
        {
            db = context;
        }

        // ✅ GET all doctors (only active)
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(db.Doctors.Where(d => d.IsActive).ToList());
        }

        // ✅ GET by ID
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null) return NotFound();

            return Ok(doctor);
        }

        // ✅ CREATE
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

        // ✅ UPDATE
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, Doctor doctor)
        {
            var existing = db.Doctors.Find(id);
            if (existing == null) return NotFound();

            db.Entry(existing).CurrentValues.SetValues(doctor);
            db.SaveChanges();

            return Ok(existing);
        }

        // ✅ DELETE
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null) return NotFound();

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok("Doctor deleted successfully");
        }
    }
}