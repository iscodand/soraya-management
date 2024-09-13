using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Application.Contracts.Services;
using Application.DTOs.Company.Requests;
using Application.DTOs.Authentication;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("empresas/")]
    public class CompanyController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public CompanyController(
            IAuthenticationService authenticationService,
            IUserService userService,
            ICompanyService companyService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _companyService = companyService;
        }

        [HttpGet("minha-empresa/")]
        public IActionResult MyCompany()
        {
            return View();
        }

        [HttpGet("empresas")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyService.GetCompaniesAsync();
            return View(companies.Data);
        }

        [HttpGet("empresas/cadastrar")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("empresas/cadastrar")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CreateCompanyRequest request)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                var result = await _companyService.CreateCompanyAsync(request);

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(GetCompanies));
                }
            }

            return View();
        }
    }
}