using System.Threading.Tasks;
using System.Web.Mvc;
using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;

namespace Healthaxis2.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        // ✅ CREATE
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.Create(dto);
            return RedirectToAction("Index");
        }

        // ✅ EDIT (IMPORTANT)
        public async Task<ActionResult> Edit(int id)
        {
            var doctor = await _service.GetById(id);
            return View(doctor);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.Update(id, dto);
            return RedirectToAction("Index");
        }

        // ✅ SEARCH
        public async Task<ActionResult> Search(string speciality)
        {
            var doctors = await _service.GetAll();
            var filtered = doctors.FindAll(d => d.Specialisation == speciality);

            return View(filtered);
        }
    }
}