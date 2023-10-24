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

        [HttpGet]
        [Route("detalhes/{mealId}")]
        public async Task<IActionResult> Details(int mealId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<DetailMealDto> result = await _mealService.DetailMealAsync(mealId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    DetailMealViewModel detailMealViewModel = new()
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        Accompaniments = result.Data.Accompaniments,
                        Orders = result.Data.Orders,
                        CreatedBy = result.Data.CreatedBy
                    };

                    return View(detailMealViewModel);
                }
            }

            return RedirectToAction(nameof(Meals));
        }

        [HttpGet]
        [Route("editar/{mealId}")]
        public async Task<IActionResult> Update(int mealId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetMealDto> result = await _mealService.GetMealByIdAsync(mealId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    UpdateMealViewModel updateMealViewModel = new()
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        Accompaniments = result.Data.Accompaniments,
                        UserCompanyId = authenticatedUser.CompanyId
                    };

                    return View(updateMealViewModel);
                }
            }

            return RedirectToAction(nameof(Meals));
        }

        [HttpPost]
        [Route("editar/{mealId}")]
        public async Task<IActionResult> Update(int mealId, UpdateMealViewModel updateMealViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                UpdateMealDto updateMealDto = new()
                {
                    Id = mealId,
                    Description = updateMealViewModel.Description,
                    Accompaniments = updateMealViewModel.Accompaniments,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<GetMealDto> result = await _mealService.UpdateMealAsync(updateMealDto);

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Meals));
                }
            }

            return View(updateMealViewModel);
        }

        [HttpDelete]
        [Route("deletar/{mealId}")]
        public async Task<IActionResult> Delete(int mealId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                DeleteMealDto deleteMealDto = new()
                {
                    Id = mealId,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<GetMealDto> result = await _mealService.DeleteMealAsync(deleteMealDto);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao deletar sabor." });
        }
    }
}