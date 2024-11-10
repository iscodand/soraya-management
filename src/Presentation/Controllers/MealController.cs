using Application.Contracts.Services;
using Application.Dtos.Meal;
using Application.DTOs.Authentication;
<<<<<<< HEAD
=======
using Application.Parameters;
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Presentation.ViewModels.Meal;

namespace Presentation.Controllers
{
    [Route("sabores/")]
    [Authorize]
    public class MealController : BaseController
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Meals(int pageNumber = 1)
        {
<<<<<<< HEAD
            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
            Response<IEnumerable<GetMealDto>> meals = await _mealService.GetMealsByCompanyAsync(authenticatedUser.CompanyId);

            List<GetMealViewModel> viewModelCollection = new();
            foreach (GetMealDto meal in meals.Data)
=======
            RequestParameter parameters = new()
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
            {
                PageNumber = pageNumber,
                PageSize = 10,
            };

            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
            var meals = await _mealService.GetByCompanyPagedAsync(
                authenticatedUser.CompanyId,
                parameters
            );

            return View(meals);
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
        public async Task<IActionResult> Create(CreateMealDto request)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                request.UserId = authenticatedUser.Id;
                request.CompanyId = authenticatedUser.CompanyId;

                Response<CreateMealDto> result = await _mealService.CreateMealAsync(request);

<<<<<<< HEAD
                Response<CreateMealDto> result = await _mealService.CreateMealAsync(createMealDto);
=======
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = true;
<<<<<<< HEAD
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        [Route("detalhes/{mealId}")]
        public async Task<IActionResult> Detail(int mealId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<DetailMealDto> result = await _mealService.DetailMealAsync(mealId, authenticatedUser.CompanyId);

                if (result.Succeeded)
=======
                    return RedirectToAction(nameof(Meals));
                }
                else
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
                {
                    return View();
                }
            }

            return RedirectToAction(nameof(Meals));
        }

        [HttpGet]
        [Route("{mealId}")]
        public async Task<IActionResult> Update(int mealId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetMealDto> result = await _mealService.GetMealByIdAsync(mealId, authenticatedUser.CompanyId);

                if (result.Succeeded)
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
        [Route("{mealId}")]
        public async Task<IActionResult> Update(int mealId, UpdateMealViewModel updateMealViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateMealDto updateMealDto = new()
                {
                    Id = mealId,
                    Description = updateMealViewModel.Description,
                    Accompaniments = updateMealViewModel.Accompaniments,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                Response<GetMealDto> result = await _mealService.UpdateMealAsync(updateMealDto);

                if (result.Succeeded)
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
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                DeleteMealDto deleteMealDto = new()
                {
                    Id = mealId,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                Response<GetMealDto> result = await _mealService.DeleteMealAsync(deleteMealDto);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao deletar sabor." });
        }
    }
}