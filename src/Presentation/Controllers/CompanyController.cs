using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Application.Dtos.User;
using Application.Responses;
using Infrastructure.Identity.Contracts;
using Infrastructure.Identity.Dtos;
using Infrastructure.Identity.Responses;
using Presentation.ViewModels.User;

namespace Presentation.Controllers
{
    [Route("minha-empresa/")]
    [Authorize(Roles = "Admin, Manager")]
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
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
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
        [Route("funcionarios/novo/")]
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
        [Route("funcionarios/novo/")]
        public async Task<IActionResult> RegisterEmployee(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                RegisterUserDto registerUserDto = new()
                {
                    Name = registerUserViewModel.Name,
                    Username = registerUserViewModel.Username,
                    Email = registerUserViewModel.Email,
                    Password = registerUserViewModel.Password,
                    ConfirmPassword = registerUserViewModel.ConfirmPassword,

                    Role = registerUserViewModel.RoleId,
                    CompanyId = authenticatedUser.CompanyId
                };

                BaseResponse result = await _authenticationService.RegisterAsync(registerUserDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return RedirectToAction(nameof(Employees));
                }
            }

            BaseResponse<GetRolesDto> roles = await _userService.GetRolesAsync();
            registerUserViewModel.RolesDropdown = roles.DataCollection;
            return View(registerUserViewModel);
        }

        [HttpGet]
        [Route("funcionarios/{employeeUsername}/")]
        public async Task<IActionResult> DetailEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<DetailUserDto> result = await _userService.DetailUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    DetailUserViewModel detailUserViewModel = new()
                    {
                        Name = result.Data.Name,
                        Email = result.Data.Email,
                        Username = result.Data.Username,
                        IsActive = result.Data.IsActive,
                        UserRole = result.Data.UserRole
                    };

                    return View(detailUserViewModel);
                }
            }

            return RedirectToAction(nameof(Employees));
        }

        [HttpPatch]
        [Route("funcionarios/ativar/{employeeUsername}")]
        public async Task<IActionResult> ActivateEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetUserDto> result = await _userService.ActivateUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao ativar funcionário." });
        }

        [HttpPatch]
        [Route("funcionarios/desativar/{employeeUsername}")]
        public async Task<IActionResult> DeactivateEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetUserDto> result = await _userService.DeactivateUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao desativar funcionário." });
        }

        [HttpDelete]
        [Route("deletar/{employeeUsername}")]
        public async Task<IActionResult> DeleteEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetUserDto> result = await _userService.DeactivateUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao deletar funcionário." });
        }
    }
}