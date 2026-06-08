using System.Threading.Tasks;
using System.Web.Mvc;
using HealthAxis_Web.Models.Dtos;
using HealthAxisWeb.Services;

namespace HealthAxisWeb.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _service;

        public DoctorController(IDoctorApiService service)
        {
            _service = service;
        }

        public async Task<ActionResult> Index()
        {
            var doctors = await _service.GetAllAsync();
            return View(doctors);
        }

        public async Task<ActionResult> Details(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            if (doctor == null)
                return HttpNotFound();

            return View(doctor);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
                return View(doctorDto);

            await _service.AddAsync(doctorDto);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            if (doctor == null)
                return HttpNotFound();

            return View(doctor);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
                return View(doctorDto);

            await _service.UpdateAsync(id, doctorDto);
            return RedirectToAction("Index");
        }
    }
}
