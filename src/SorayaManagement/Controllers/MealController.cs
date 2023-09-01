using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Meal;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Responses;
using SorayaManagement.ViewModels;

namespace SorayaManagement.Controllers
{
    [Route("sabores/")]
    [Authorize]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly ISessionService _sessionService;

        public MealController(IMealService mealService, ISessionService sessionService)
        {
            _mealService = mealService;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Meals()
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();
            ICollection<Meal> meals = await _mealService.GetMealsByCompanyAsync(authenticatedUser.CompanyId);

            // todo => improve and clean this logic
            List<GetMealsViewModel> viewModelCollection = new();
            foreach (Meal meal in meals)
            {
                GetMealsViewModel viewModel = new()
                {
                    Id = meal.Id,
                    Description = meal.Description,
                    Accompaniments = meal.Accompaniments,
                    CreatedBy = meal.User.Name
                };

                viewModelCollection.Add(viewModel);
            }

            return View(viewModelCollection);
        }

        [HttpGet]
        [Route("novo/")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("novo/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMealDto createMealDto)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse result = await _mealService.CreateMealAsync(createMealDto, authenticatedUser);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return View();
                }
            }

            return View();
        }
    }
}