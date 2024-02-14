using Application.Contracts.Services;
using Application.Dtos.Data;
using Application.DTOs.Authentication;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;

namespace Presentation.Controllers
{
    [Route("/")]
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IDataService _dataService;

        public HomeController(IAuthenticatedUserService authenticatedUserService,
                              IDataService dataService)
        {
            _authenticatedUserService = authenticatedUserService;
            _dataService = dataService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Home()
        {
            GetAuthenticatedUserDto authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            SessionService.AddUserSession(authenticatedUser);
            return View(authenticatedUser);
        }

        // TODO => get a better name for controller and route
        [HttpGet]
        [Route("data/")]
        public async Task<IActionResult> GetData(string selectedDate, DateTime initialDate)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                DateTime today = DateTime.Today.Date;

                Dictionary<string, int> dateMappings = new()
                {
                    { "today", 0 },
                    { "lastWeek", -7 },
                    { "last15Days", -15 },
                    { "lastMonth", -365 }
                };

                if (dateMappings.TryGetValue(selectedDate, out int daysToSubtract))
                {
                    initialDate = today.AddDays(daysToSubtract);
                }

                BaseResponse<GetDataDto> result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, initialDate, today);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message, data = result.Data });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Ocorreu um erro ao processar a solicitação" });
        }
    }
}