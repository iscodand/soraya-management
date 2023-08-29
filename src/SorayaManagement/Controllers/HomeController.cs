using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.ViewModels;

namespace SorayaManagement.Controllers
{
    [Route("")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public HomeController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        [Route("home/")]
        public async Task<IActionResult> Home()
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUser();

            HomeViewModel viewModel = new()
            {
                UserName = authenticatedUser.Name,
                CompanyName = authenticatedUser.UserCompany.Name
            };

            return View(viewModel);
        }
    }
}