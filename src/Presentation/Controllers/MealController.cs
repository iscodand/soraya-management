using Application.Contracts;
using Application.Dtos.Meal;
using Application.Dtos.User;
using Application.Responses;
using Infrastructure.Identity.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Meal;

namespace Presentation.Controllers
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
            GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
            BaseResponse<GetMealDto> meals = await _mealService.GetMealsByCompanyAsync(authenticatedUser.CompanyId);

            List<GetMealViewModel> viewModelCollection = new();
            foreach (GetMealDto meal in meals.DataCollection)
            {
                GetMealViewModel viewModel = new()
                {
                    Id = meal.Id,
                    Description = meal.Description,
                    Accompaniments = meal.Accompaniments,
                    CreatedBy = meal.CreatedBy
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
        public async Task<IActionResult> Create(CreateMealViewModel createMealViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                CreateMealDto createMealDto = new()
                {
                    Description = createMealViewModel.Description,
                    Accompaniments = createMealViewModel.Accompaniments,
                    CompanyId = authenticatedUser.CompanyId,
                    UserId = authenticatedUser.Id
                };

                BaseResponse<CreateMealDto> result = await _mealService.CreateMealAsync(createMealDto);
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