using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Identity.Contracts;
using Application.Dtos.User;
using Application.Contracts;
using Application.Responses;
using Application.Dtos.Data;
using System.Globalization;

namespace Presentation.Controllers
{
    [Route("/")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ISessionService _sessionService;
        private readonly IDataService _dataService;

        public HomeController(IAuthenticatedUserService authenticatedUserService,
                              ISessionService sessionService,
                              IDataService dataService)
        {
            _authenticatedUserService = authenticatedUserService;
            _sessionService = sessionService;
            _dataService = dataService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Home()
        {
            GetAuthenticatedUserDto authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            _sessionService.AddUserSession(authenticatedUser);
            return View(authenticatedUser);
        }

        // TODO => get a better name for controller and route
        [HttpGet]
        [Route("data/")]
        public async Task<IActionResult> GetData(DateTime initialDate, DateTime finalDate)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                initialDate = DateTime.ParseExact("16/10/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                BaseResponse<GetDataDto> result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, initialDate, DateTime.Today);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message, data = result.Data });
                }
            }
            return Json(new { success = false, message = "Ocorreu um erro ao processar a solicitação" });
        }
    }
}