using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Application.Contracts.Services;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [Route("minha-empresa/")]
    public class CompanyController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public CompanyController(
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        public IActionResult MyCompany()
        {
            return View();
        }
    }
}