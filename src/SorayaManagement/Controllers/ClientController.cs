using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SorayaManagement.Controllers
{
    [Authorize]
    [Route("clientes")]
    public class ClientController : Controller
    {
        public ClientController()
        {

        }

        [Route("")]
        public IActionResult Clients()
        {
            return View();
        }
    }
}