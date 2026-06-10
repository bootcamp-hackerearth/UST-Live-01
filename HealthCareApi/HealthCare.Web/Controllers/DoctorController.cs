using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;
        private const int PageSize = 10;

        public DoctorController()
        {
            _service = new DoctorService();
        }


        public ActionResult Index()
        {
            return View();  
        }

        //  LIST → Doctor/List.cshtml
        public async Task<ActionResult> List(
            string specialization,
            string searchTerm,
            bool orderByDescending = false,
            int pageNumber = 1)
        {
            var result = await _service.GetDoctorsAsync(
                specialization,
                searchTerm,
                orderByDescending,
                pageNumber,
                PageSize);

            return View(result);
        }

        //  PROFILE 
        public async Task<ActionResult> Profile(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return HttpNotFound();

            return View("Profile", doctor);
        }

        //  CREATE 


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid doctor data";
                return RedirectToAction("List"); 
            }

            var result = await _service.CreateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Doctor added successfully.";
            }
            else
            {
                TempData["Error"] = "Error adding doctor.";
            }

            return RedirectToAction("List"); 
        }



        //  EDIT (GET) 
        public async Task<ActionResult> Edit(int id)
          {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return HttpNotFound();

            return View("Edit", doctor);
        }

        //  EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return View("Edit", dto);

            var result = await _service.UpdateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Doctor updated successfully.";
                return RedirectToAction("Profile", new { id = dto.DoctorId });
            }

            ModelState.AddModelError("", "Error updating doctor");
            return View("Edit", dto);
        }

        //  DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result)
                TempData["Success"] = "Doctor deleted.";
            else
                TempData["Error"] = "Delete failed.";

            return RedirectToAction("Index");
        }
    }
}