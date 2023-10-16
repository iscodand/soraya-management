using Microsoft.AspNetCore.Mvc;
using Infrastructure.Identity.Contracts;
using Infrastructure.Identity.Dtos;
using Infrastructure.Identity.Responses;
using Presentation.ViewModels.Authentication;

namespace Presentation.Controllers
{
    [Route("auth/")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionService _sessionService;

        public AuthenticationController(IAuthenticationService authenticationService, ISessionService sessionService)
        {
            _authenticationService = authenticationService;
            _sessionService = sessionService;
        }

        // auth/login
        [HttpGet]
        [Route("login/")]
        public IActionResult Login()
        {
            return View();
        }

        // auth/login
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
            _sessionService.RemoveUserSession();
            return RedirectToAction("Login");
        }
    }
}