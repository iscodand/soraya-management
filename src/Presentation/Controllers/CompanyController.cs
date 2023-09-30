using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dto.User;
using SorayaManagement.Application.Dtos;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Dtos;
using SorayaManagement.Infrastructure.Identity.Responses;
using SorayaManagement.UI.ViewModels.User.Employee;
using SorayaManagement.ViewModels;

namespace SorayaManagement.UI.Controllers
{
    [Route("minha-empresa/")]
    // [Authorize(Roles = "Manager, Admin")]
    public class CompanyController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public CompanyController(
            ISessionService sessionService,
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            _sessionService = sessionService;
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult MyCompany()
        {
            return View();
        }

        [HttpGet]
        [Route("funcionarios/")]
        public async Task<IActionResult> Employees()
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetUserDto> result = await _userService.GetUsersByCompanyAsync(authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    List<GetUserViewModel> getUserViewModelCollection = new();

                    foreach (GetUserDto getUserDto in result.DataCollection)
                    {
                        GetUserViewModel getUserViewModel = new()
                        {
                            Name = getUserDto.Name,
                            Username = getUserDto.Username,
                            Email = getUserDto.Email
                        };

                        getUserViewModelCollection.Add(getUserViewModel);
                    }

                    return View(getUserViewModelCollection);
                }
            }

            return View(nameof(MyCompany));
        }

        // auth/cadastro/
        [HttpGet]
        [Route("funcionario/novo/")]
        public async Task<IActionResult> RegisterEmployee()
        {
            BaseResponse<GetRolesDto> roles = await _userService.GetRolesAsync();

            RegisterUserViewModel registerUserViewModel = new()
            {
                RolesDropdown = roles.DataCollection
            };

            return View(registerUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("funcionario/novo/")]
        public async Task<IActionResult> RegisterEmployee(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                // User authenticatedUser = _sessionService.RetrieveUserSession();

                RegisterUserDto registerUserDto = new()
                {
                    Name = registerUserViewModel.Name,
                    Username = registerUserViewModel.Username,
                    Email = registerUserViewModel.Email,
                    Password = registerUserViewModel.Password,
                    ConfirmPassword = registerUserViewModel.ConfirmPassword,

                    CompanyId = 1
                };

                BaseResponse result = await _authenticationService.RegisterAsync(registerUserDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return Redirect(nameof(Employees));
                }
            }

            return View();
        }
    }
}