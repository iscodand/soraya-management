using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Dtos;

namespace SorayaManagement.Controllers
{
    [Route("/")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ISessionService _sessionService;

        public HomeController(IAuthenticatedUserService authenticatedUserService, ISessionService sessionService)
        {
            _authenticatedUserService = authenticatedUserService;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Home()
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            _sessionService.AddUserSession(authenticatedUser);
            return View();
        }

        // // auth/cadastro/
        // [Authorize]
        // [HttpGet]
        // [Route("cadastro/")]
        // public IActionResult Register()
        // {
        //     return View();
        // }

        // [Authorize]
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // [Route("cadastro/")]
        // public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         BaseResponse result = await _authenticationService.RegisterAsync(registerUserDto);
        //         ViewData["Message"] = result.Message;

        //         if (result.IsSuccess)
        //         {
        //             return Redirect(nameof(Login));
        //         }
        //     }

        //     return View();
        // }
    }
}