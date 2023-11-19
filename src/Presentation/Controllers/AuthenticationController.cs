using Microsoft.AspNetCore.Mvc;
using Infrastructure.Identity.Contracts;
using Infrastructure.Identity.Dtos;
using Infrastructure.Identity.Responses;
using Presentation.ViewModels.Authentication;
using Presentation.Controllers.Common;

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

                BaseResponse result = await _authenticationService.LoginAsync(loginUserDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    return RedirectToAction("home", "Home");
                }
            }

            return View();
        }

        // auth/logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            SessionService.RemoveUserSession();
            return RedirectToAction("Login");
        }
    }
}