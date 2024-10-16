using Application.Contracts.Services;
using Application.Dtos.Data;
using Application.DTOs.Authentication;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;

namespace Presentation.Controllers
{
    [Route("/")]
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Home()
        {
            GetAuthenticatedUserDto authenticatedUser = await AuthenticatedUser.GetAuthenticatedUserAsync();
            SessionService.AddUserSession(authenticatedUser);

            if (authenticatedUser is not null)
            {
                return View(authenticatedUser);
            }

            return RedirectToAction("Login", "Authentication");
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

                Response<GetDataDto> result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, initialDate, today);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message, data = result.Data });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Ocorreu um erro ao processar a solicitação" });
        }
    }
}