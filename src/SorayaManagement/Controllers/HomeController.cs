using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SorayaManagement.Controllers
{
    [Route("/   ")]
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [HttpGet]
        [Route("/")]
        public IActionResult Home()
        {
            return View();
        }
    }
}