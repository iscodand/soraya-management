using Application.Contracts;
using Application.Dtos.Meal;
using Application.Dtos.User;
using Application.Responses;
using Infrastructure.Identity.Contracts;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using Presentation.ViewModels.Meal;

namespace Tests.Controllers
{
    public class MealControllerTest
    {
        private MealController _mealController;
        private IMealService _mealService;
        private ISessionService _sessionService;

        public MealControllerTest()
        {
            _mealService = A.Fake<IMealService>();
            _sessionService = A.Fake<ISessionService>();
            _mealController = new MealController(_mealService, _sessionService);
        }

        // [HttpGet]
        // [Route("editar/{customerId}")]
        // public async Task<IActionResult> Update(int customerId)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
        //         BaseResponse<GetCustomerDto> result = await _customerService.GetCustomerByIdAsync(customerId, authenticatedUser.CompanyId);

        //         if (result.IsSuccess)
        //         {
        //             UpdateCustomerViewModel updateCustomerViewModel = new()
        //             {
        //                 Id = result.Data.Id,
        //                 Name = result.Data.Name,
        //                 Phone = result.Data.Phone
        //             };

        //             return View(updateCustomerViewModel);
        //         }
        //     }

        //     return RedirectToAction(nameof(Customers));
        // }

        //     return View();
        // }


        // Scenarios - Update Meal
        // 1 - Valid update
        // 2 - Invalid update

        [Fact]
        public async Task UpdateMeal_ValidUpdate_ReturnsSuccess()
        {
            // Arrange
            GetAuthenticatedUserDto authenticatedUser = A.Fake<GetAuthenticatedUserDto>();
            authenticatedUser.CompanyId = 1;

            UpdateMealViewModel updateMealViewModel = new()
            {
                Id = 1,
                Description = "Testing",
                Accompaniments = "Testing Accompaniments",
                UserCompanyId = authenticatedUser.CompanyId
            };

            UpdateMealDto updateMealDto = new()
            {
                Id = updateMealViewModel.Id,
                Description = updateMealViewModel.Description,
                Accompaniments = updateMealViewModel.Accompaniments,
                UserCompanyId = updateMealViewModel.UserCompanyId
            };

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor atualizado com sucesso",
                IsSuccess = true
            };

            // Act
            A.CallTo(() => _sessionService.RetrieveUserSession()).Returns(authenticatedUser);
            A.CallTo(() => _mealService.UpdateMealAsync(updateMealDto)).Returns(response);

            var result = await _mealController.Update(updateMealViewModel.Id, updateMealViewModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task UpdateMeal_InvalidUpdate_ReturnsError()
        {
            // Arrange
            GetAuthenticatedUserDto authenticatedUser = A.Fake<GetAuthenticatedUserDto>();
            authenticatedUser.CompanyId = 1;

            UpdateMealViewModel updateMealViewModel = new()
            {
                Id = 1,
                Description = "Testing",
                Accompaniments = "Testing Accompaniments",
                UserCompanyId = 2
            };

            UpdateMealDto updateMealDto = new()
            {
                Id = updateMealViewModel.Id,
                Description = updateMealViewModel.Description,
                Accompaniments = updateMealViewModel.Accompaniments,
                UserCompanyId = updateMealViewModel.UserCompanyId
            };

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Esse sabor não pertence a sua empresa",
                IsSuccess = false
            };

            // Act
            A.CallTo(() => _sessionService.RetrieveUserSession()).Returns(authenticatedUser);
            A.CallTo(() => _mealService.UpdateMealAsync(updateMealDto)).Returns(response);

            var result = await _mealController.Update(updateMealViewModel.Id, updateMealViewModel);

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task DeleteMeal_ValidDelete_ReturnsSuccess()
        {
            // Arrange
            GetAuthenticatedUserDto authenticatedUser = A.Fake<GetAuthenticatedUserDto>();
            authenticatedUser.CompanyId = 1;

            GetMealViewModel getMealViewModel = new()
            {
                Id = 1
            };

            DeleteMealDto deleteMealDto = new()
            {
                Id = getMealViewModel.Id,
                UserCompanyId = authenticatedUser.CompanyId
            };

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor deletado com sucesso",
                IsSuccess = true
            };

            // Act
            A.CallTo(() => _sessionService.RetrieveUserSession()).Returns(authenticatedUser);
            A.CallTo(() => _mealService.DeleteMealAsync(deleteMealDto)).Returns(response);

            var result = await _mealController.Delete(getMealViewModel.Id);

            // Assert
            result.Should().BeOfType<JsonResult>();
        }

        [Fact]
        public async Task DeleteMeal_InvalidDelete_ReturnsError()
        {
            // Arrange
            GetAuthenticatedUserDto authenticatedUser = A.Fake<GetAuthenticatedUserDto>();
            authenticatedUser.CompanyId = 1;

            GetMealViewModel getMealViewModel = new()
            {
                Id = 1
            };

            DeleteMealDto deleteMealDto = new()
            {
                Id = getMealViewModel.Id,
                UserCompanyId = 2
            };

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Esse sabor não pertence a sua empresa",
                IsSuccess = false
            };

            // Act
            A.CallTo(() => _sessionService.RetrieveUserSession()).Returns(authenticatedUser);
            A.CallTo(() => _mealService.DeleteMealAsync(deleteMealDto)).Returns(response);

            var result = await _mealController.Delete(getMealViewModel.Id);

            // Assert
            result.Should().BeOfType<JsonResult>();
        }
    }
}