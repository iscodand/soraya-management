using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Dtos;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Controllers
{
    [Route("minha-empresa/")]
    [Authorize(Roles = "Manager, Admin")]
    public class CompanyController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IAuthenticationService _authenticationService;

        public CompanyController(ISessionService sessionService, IAuthenticationService authenticationService)
        {
            _sessionService = sessionService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult MyCompany()
        {
            return View();
        }

        [HttpGet]
        [Route("funcionarios/")]
        public IActionResult Employees()
        {
            return View();
        }

        // auth/cadastro/
        [HttpGet]
        [Route("funcionario/novo/")]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("funcionario/novo/")]
        public async Task<IActionResult> RegisterEmployee(RegisterUserDto registerUserDto)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();

                BaseResponse result = await _authenticationService.RegisterAsync(registerUserDto, authenticatedUser);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    return Redirect(nameof(Employees));
                }
            }

            return View();
        }
    }
}