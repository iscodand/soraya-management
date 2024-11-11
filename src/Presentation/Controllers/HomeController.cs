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
                DateTime today = DateTime.Today.Date;

                var result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, today, today);

                return View(result.Data);
            }

            return RedirectToAction("Login", "Authentication");
        }

        // TODO => get a better name for controller and route
        [HttpGet]
        [Route("data/")]
        public async Task<IActionResult> GetData(string selectedDate, DateTime initialDate)
        {
            // if (ModelState.IsValid)
            // {
            //     GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            //     DateTime today = DateTime.Today.Date;

            //     Dictionary<string, int> dateMappings = new()
            //     {
            //         { "today", 0 },
            //         { "lastWeek", -7 },
            //         { "last15Days", -15 },
            //         { "lastMonth", -365 }
            //     };

            //     if (dateMappings.TryGetValue(selectedDate, out int daysToSubtract))
            //     {
            //         initialDate = today.AddDays(daysToSubtract);
            //     }

<<<<<<< HEAD
                Response<GetDataDto> result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, initialDate, today);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message, data = result.Data });
                }
=======
            //     Response<GetDataDto> result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, initialDate, today);

            //     if (result.Succeeded)
            //     {
            //         return Json(new { success = true, message = result.Message, data = result.Data });
            //     }
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650

            //     return Json(new { success = false, message = result.Message });
            // }

            return Json(new { success = false, message = "Ocorreu um erro ao processar a solicitação" });
        }
    }
}