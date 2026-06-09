using HealthAxis.Shared.Dtos;
using HealthAxisWeb.Services;
using System.Collections.Generic;
using System.Linq;  
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxisWeb.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _service;

        public DoctorController(IDoctorApiService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> List()
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
            return RedirectToAction("List");
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
            return RedirectToAction("List");   
        }

        public async Task<ActionResult> SearchById(int? id)
        {
            DoctorDto doctor = null;

            if (id.HasValue)
            {
                doctor = await _service.GetByIdAsync(id.Value);

                if (doctor == null)
                    ViewBag.Message = "Doctor not found";
            }

            return View(doctor);
        }

        public async Task<ActionResult> SearchBySpecialisation(DoctorDto.SpecialisationType? specialisation)
        {
            var doctors = new List<DoctorDto>();

            if (specialisation.HasValue)
            {
                var allDoctors = await _service.GetAllAsync();

                doctors = allDoctors
                    .Where(d => d.Specialisation == specialisation.Value)
                    .ToList();

                if (!doctors.Any())
                    ViewBag.Message = "No doctors found";
            }

            return View(doctors);
        }
    }
}