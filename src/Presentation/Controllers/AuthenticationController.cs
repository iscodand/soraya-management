using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Authentication;
using Presentation.Controllers.Common;
using Application.DTOs.Authentication;
using Application.Responses;
using Application.Contracts.Services;

namespace Presentation.Controllers
{
    [Route("auth/")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("login/")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login/")]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                LoginUserDto loginUserDto = new()
                {
                    UserName = loginUserViewModel.UserName,
                    Password = loginUserViewModel.Password
                };

                BaseResponse<string> result = await _authenticationService.LoginAsync(loginUserDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    return RedirectToAction("home", "Home");
                }
            }

            return View();
        }

        [HttpGet("recuperar-senha")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                BaseResponse<string> result = await _authenticationService.ForgotPasswordAsync(viewModel.Email, Request.Headers.Origin.ToString());
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
            }

            return View();
        }

        [HttpGet("recuperar-senha/confirm")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            SessionService.RemoveUserSession();
            return RedirectToAction(nameof(Login));
        }
    }
}