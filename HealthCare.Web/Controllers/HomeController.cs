using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthCare.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}