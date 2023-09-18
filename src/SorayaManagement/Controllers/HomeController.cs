using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;

namespace SorayaManagement.Controllers
{
    [Route("/")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ISessionService _sessionService;

        public HomeController(IAuthenticatedUserService authenticatedUserService, ISessionService sessionService)
        {
            _authenticatedUserService = authenticatedUserService;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Home()
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            _sessionService.AddUserSession(authenticatedUser);
            return View(authenticatedUser);
        }
    }
}