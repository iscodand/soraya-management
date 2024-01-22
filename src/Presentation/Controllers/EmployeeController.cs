using Application.Contracts;
using Application.Dtos.User;
using Application.DTOs.Authentication;
using Application.Responses;
using Infrastructure.Identity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Presentation.ViewModels.User;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
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
                        Username = employeeUsername,
                        NewUsername = result.Data.Username,
                        Name = result.Data.Name,
                        NewEmail = result.Data.Email,
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
                    NewUsername = updateUserViewModel.NewUsername,
                    NewEmail = updateUserViewModel.NewEmail,
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