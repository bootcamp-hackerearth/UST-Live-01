using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Healthaxis2.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
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
        public async Task<ActionResult> Create(PatientDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false });

            var patient = await _service.CreateAndReturn(dto); // ✅ IMPORTANT

            return Json(new
            {
                success = true,
                data = patient
            });
        }


        public ActionResult Details()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Details(int id)
        {
            var patient = await _service.GetById(id);
            return View("PatientDetailsResult", patient);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _service.GetById(id);
            return View(patient);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, PatientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _service.Update(id, dto);
            return RedirectToAction("Index");
        }
        // ✅ SINGLE SEARCH (ID / NAME / PHONE)
        [HttpPost]
        public async Task<ActionResult> Search(string searchTerm)
        {
            var patients = await _service.GetAll();

            var result = patients.Where(p =>
                p.PatientId.ToString().Contains(searchTerm) ||
                p.PhoneNumber.Contains(searchTerm) ||
                p.PatientName.ToLower().Contains(searchTerm.ToLower())
            ).ToList();

            return View("SearchResult", result);
        }
    }
}