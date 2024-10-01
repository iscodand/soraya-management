using Application.Contracts.Services;
using Application.Dtos.User;
using Application.DTOs.Authentication;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Presentation.Controllers.Common;
using Presentation.ViewModels.User;

namespace Presentation.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Manager")]
    [Route("minha-empresa/funcionarios/")]
    public class EmployeeController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public EmployeeController(
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Employees()
        {
            if (ModelState.IsValid)
            {
                var authenticatedUser = SessionService.RetrieveUserSession();
                var result = await _userService.GetUsersByCompanyAsync(authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
            }

            return View("Home");
        }

        // auth/cadastro/
        [HttpGet]
        [Route("novo/")]
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
        [Route("novo/")]
        public async Task<IActionResult> RegisterEmployee(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

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

                BaseResponse<string> result = await _authenticationService.RegisterAsync(registerUserDto);
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
        [Route("{employeeUsername}/")]
        public async Task<IActionResult> DetailEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                BaseResponse<DetailUserDto> result = await _userService.DetailUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
            }

            return RedirectToAction(nameof(Employees));
        }

        [HttpGet]
        [Route("editar/{employeeUsername}")]
        public async Task<IActionResult> UpdateEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                BaseResponse<DetailUserDto> result = await _userService.DetailUserAsync(employeeUsername, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    UpdateUserViewModel updateUserViewModel = new()
                    {
                        Id = result.Data.Id,
                        Username = employeeUsername,
                        Name = result.Data.Name,
                        Email = result.Data.Email,
                    };

                    return View(updateUserViewModel);
                }
            }

            return View(nameof(Employees));
        }

        [HttpPost]
        [Route("editar/{employeeUsername}")]
        public async Task<IActionResult> UpdateEmployee(string employeeUsername, UpdateUserViewModel updateUserViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateUserDto updateUserDto = new()
                {
                    Name = updateUserViewModel.Name,
                    Username = employeeUsername,
                    Email = updateUserViewModel.Email,
                    CompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<UpdateUserDto> result = await _userService.UpdateUserAsync(updateUserDto);

                ViewData["Message"] = result.Message;
                ViewData["IsSuccess"] = result.IsSuccess;

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Employees));
                }
            }

            return View(updateUserViewModel);
        }

        [HttpPatch]
        [Route("ativar/{employeeUsername}")]
        public async Task<IActionResult> ActivateEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
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
        [Route("desativar/{employeeUsername}")]
        public async Task<IActionResult> DeactivateEmployee(string employeeUsername)
        {

            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
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

        [HttpGet]
        [Route("{employeeUsername}/alterar-senha")]
        public async Task<IActionResult> ChangePasswordAsync(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetUserByUsernameAsync(employeeUsername);
                if (result.IsSuccess)
                {
                    ChangePasswordDto request = new()
                    {
                        Username = result.Data.Username,
                        OldPassword = "",
                        NewPassword = "",
                        ConfirmNewPassword = ""
                    };

                    return View(request);
                }
            }

            return RedirectToAction(nameof(Employees));
        }


        [HttpPost]
        [Route("{employeeUsername}/alterar-senha")]
        public async Task<IActionResult> ChangePasswordAsync(string employeeUsername, ChangePasswordDto request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.ChangePasswordAsync(employeeUsername, request);

                ViewData["Message"] = result.Message;
                ViewData["IsSuccess"] = result.IsSuccess;
            }

            request.Username = employeeUsername;
            return View(request);
        }

        [HttpDelete]
        [Route("deletar/{employeeUsername}")]
        public async Task<IActionResult> DeleteEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
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