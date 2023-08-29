using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Dtos;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Controllers
{
    [Route("auth/")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // auth/cadastro/
        [Authorize]
        [HttpGet]
        [Route("cadastro/")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("cadastro/")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (ModelState.IsValid)
            {
                BaseResponse result = await _authenticationService.RegisterAsync(registerUserDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    return Redirect(nameof(Login));
                }
            }

            return View();
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
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (ModelState.IsValid)
            {
                BaseResponse result = await _authenticationService.LoginAsync(loginUserDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    return RedirectToAction("home", "Home");
                }
            }

            return View();
        }

    }
}