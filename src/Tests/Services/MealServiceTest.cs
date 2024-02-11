using Application.Services;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Data.Repositories;
using Application.Dtos.Meal;
using Application.Dtos.Order;
using Application.Contracts.Services;

namespace Tests.Services
{
    public class MealServiceTest
    {
        private readonly IMealService _mealService;
        private readonly IMealRepository _mealRepository;

        public MealServiceTest()
        {
            _mealRepository = A.Fake<IMealRepository>();
            _mealService = new MealService(_mealRepository);
        }

        // Scenarios - Update Meal
        // 1 - Valid update
        // 2 - Invalid CompanyId
        // 3 - Null meal (not found)
        // 4 - Null UpdateMealDto
        // 5 - Update Meal with existent Description by Company

        [Fact]
        public async Task UpdateMeal_ValidUpdate_ReturnsSuccess()
        {
            // Arrange
            Meal meal = A.Fake<Meal>();
            meal.CompanyId = 1;

            UpdateMealDto updateMealDto = new()
            {
                Id = meal.Id,
                Description = "Testing",
                Accompaniments = "Test Accompaniments",
                UserCompanyId = meal.CompanyId
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(updateMealDto.Id)).Returns(meal);
            A.CallTo(() => _mealRepository.MealExistsByDescriptionAsync(updateMealDto.Description, updateMealDto.UserCompanyId)).Returns(false);
            A.CallTo(() => meal.Update(updateMealDto.Description, updateMealDto.Accompaniments));
            A.CallTo(() => _mealRepository.SaveAsync());

            var result = await _mealService.UpdateMealAsync(updateMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor atualizado com sucesso",
                IsSuccess = true
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateMeal_InvalidCompany_ReturnsError()
        {
            // Arrange
            Meal meal = A.Fake<Meal>();
            meal.CompanyId = 1;

            UpdateMealDto updateMealDto = new()
            {
                Id = meal.Id,
                Description = "Testing",
                Accompaniments = "Test Accompaniments",
                UserCompanyId = 2
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(updateMealDto.Id)).Returns(meal);

            var result = await _mealService.UpdateMealAsync(updateMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Esse sabor não pertence a sua empresa",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateMeal_NullMeal_ReturnsError()
        {
            // Arrange
            Meal? meal = null;

            UpdateMealDto? updateMealDto = new()
            {
                Id = 1,
                Description = "Testing",
                Accompaniments = "Test Accompaniments",
                UserCompanyId = 1
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(updateMealDto.Id)).Returns(meal);

            var result = await _mealService.UpdateMealAsync(updateMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor não encontrado",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateMeal_NullUpdateMealDto_ReturnsError()
        {
            // Arrange
            UpdateMealDto? updateMealDto = null;

            // Act
            var result = await _mealService.UpdateMealAsync(updateMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor não pode ser nulo",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateMeal_ExistentDescriptionByCompany_ReturnsError()
        {
            Meal meal = A.Fake<Meal>();
            meal.CompanyId = 1;

            UpdateMealDto updateMealDto = new()
            {
                Id = meal.Id,
                Description = "Testing",
                Accompaniments = "Test Accompaniments",
                UserCompanyId = meal.CompanyId
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(updateMealDto.Id)).Returns(meal);
            A.CallTo(() => _mealRepository.MealExistsByDescriptionAsync(updateMealDto.Description, updateMealDto.UserCompanyId)).Returns(true);

            var result = await _mealService.UpdateMealAsync(updateMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Um sabor já foi cadastrado com essa Descrição. Verifique e tente novamente",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        // Scenarios - Get Meal By Id
        // 1 - Valid meal
        // 2 - Invalid CompanyId
        // 3 - Null meal (not found)

        [Fact]
        public async Task GetMealById_ValidMeal_ReturnsSuccess()
        {
            // Arrange
            Meal meal = A.Fake<Meal>();
            meal.Id = 1;
            meal.CompanyId = 1;

            GetMealDto getMealDto = new()
            {
                Id = meal.Id,
                Description = meal.Description,
                Accompaniments = meal.Accompaniments,
                CreatedBy = meal.UserId
            };

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor encontrado com sucesso",
                IsSuccess = true,
                Data = getMealDto
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(meal.Id)).Returns(meal);

            var result = await _mealService.GetMealByIdAsync(meal.Id, 1);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
            result.Data.Should().BeEquivalentTo(response.Data);
        }

        [Fact]
        public async Task GetMealById_InvalidCompanyId_ReturnsError()
        {
            // Arrange
            Meal meal = A.Fake<Meal>();
            meal.CompanyId = 1;

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Esse sabor não pertence a sua empresa",
                IsSuccess = false,
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(meal.Id)).Returns(meal);

            var result = await _mealService.GetMealByIdAsync(meal.Id, 2);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
            result.Data.Should().Be(response.Data);
        }

        [Fact]
        public async Task GetMealById_NullMeal_ReturnsError()
        {
            // Arrange
            Meal? meal = null;

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor não encontrado",
                IsSuccess = false,
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(1)).Returns(meal);

            var result = await _mealService.GetMealByIdAsync(1, 1);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
            result.Data.Should().Be(response.Data);
        }

        // Scenarios - Detail Meal
        // 1 - Valid detail
        // 2 - Invalid CompanyId
        // 3 - Null meal (not found)

        [Fact]
        public async Task DetailMeal_ValidDetail_ReturnsSuccess()
        {
            // Arrange
            User user = A.Fake<User>();
            Meal meal = A.Fake<Meal>();
            meal.Id = 1;
            meal.CompanyId = 1;
            meal.User = user;

            ICollection<GetOrderDto> orders = A.Fake<ICollection<GetOrderDto>>();

            DetailMealDto detailMealDto = new()
            {
                Id = meal.Id,
                Description = meal.Description,
                Accompaniments = meal.Accompaniments,
                Orders = orders,
                CreatedBy = meal.User.Name,
            };

            BaseResponse<DetailMealDto> response = new()
            {
                Message = "Sabor encontrado com sucesso",
                IsSuccess = true,
                Data = detailMealDto
            };

            // Act
            A.CallTo(() => _mealRepository.DetailMealAsync(detailMealDto.Id)).Returns(meal);

            var result = await _mealService.DetailMealAsync(meal.Id, 1);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
            result.Data.Should().BeEquivalentTo(response.Data);
        }

        [Fact]
        public async Task DetailMeal_InvalidCompanyId_ReturnsSuccess()
        {
            // Arrange
            User user = A.Fake<User>();
            Meal meal = A.Fake<Meal>();
            meal.Id = 1;
            meal.CompanyId = 1;
            meal.User = user;

            ICollection<GetOrderDto> orders = A.Fake<ICollection<GetOrderDto>>();

            DetailMealDto detailMealDto = new()
            {
                Id = meal.Id,
                Description = meal.Description,
                Accompaniments = meal.Accompaniments,
                Orders = orders,
                CreatedBy = meal.User.Name,
            };

            BaseResponse<DetailMealDto> response = new()
            {
                Message = "Esse sabor não pertence a sua empresa",
                IsSuccess = false,
            };

            // Act
            A.CallTo(() => _mealRepository.DetailMealAsync(detailMealDto.Id)).Returns(meal);

            var result = await _mealService.DetailMealAsync(meal.Id, 2);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
            result.Data.Should().Be(response.Data);
        }

        [Fact]
        public async Task DetailMeal_NullMeal_ReturnsSuccess()
        {
            // Arrange
            Meal? meal = null;

            BaseResponse<DetailMealDto> response = new()
            {
                Message = "Sabor não encontrado",
                IsSuccess = false,
            };

            // Act
            A.CallTo(() => _mealRepository.DetailMealAsync(1)).Returns(meal);

            var result = await _mealService.DetailMealAsync(1, 1);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
            result.Data.Should().Be(response.Data);
        }

        // Scenarios - Delete Meal
        // 1 - Valid delete
        // 2 - Invalid CompanyId
        // 3 - Orders registered with Meal
        // 4 - Null meal (not found)
        // 5 - Null DeleteMealDto

        [Fact]
        public async Task DeleteMeal_ValidDelete_ReturnsSuccess()
        {
            Meal meal = A.Fake<Meal>();
            meal.CompanyId = 1;

            // Arrange
            DeleteMealDto? deleteMealDto = new()
            {
                Id = meal.Id,
                UserCompanyId = meal.CompanyId
            };

            // Act
            A.CallTo(() => _mealRepository.DetailMealAsync(deleteMealDto.Id)).Returns(meal);
            A.CallTo(() => _mealRepository.DeleteAsync(meal));

            var result = await _mealService.DeleteMealAsync(deleteMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor deletado com sucesso",
                IsSuccess = true
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteMeal_InvalidCompanyId_ReturnsError()
        {
            Meal meal = A.Fake<Meal>();
            meal.CompanyId = 1;

            // Arrange
            DeleteMealDto? deleteMealDto = new()
            {
                Id = meal.Id,
                UserCompanyId = 2
            };

            // Act
            A.CallTo(() => _mealRepository.GetByIdAsync(deleteMealDto.Id)).Returns(meal);

            var result = await _mealService.DeleteMealAsync(deleteMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Esse sabor não pertence a sua empresa",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteMeal_MealVinculatedToOrders_ReturnsError()
        {
            Meal meal = A.Fake<Meal>();
            Order order = A.Fake<Order>();
            List<Order> orders = A.Fake<List<Order>>();
            orders.Add(order);

            meal.CompanyId = 1;
            meal.Orders = orders;

            // Arrange
            DeleteMealDto? deleteMealDto = new()
            {
                Id = meal.Id,
                UserCompanyId = meal.CompanyId
            };

            // Act
            A.CallTo(() => _mealRepository.DetailMealAsync(deleteMealDto.Id)).Returns(meal);

            var result = await _mealService.DeleteMealAsync(deleteMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = $"Você não pode deletar esse sabor, pois ele está vinculado a {orders.Count} pedido(s)",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteMeal_NullMeal_ReturnsError()
        {
            Meal? meal = null;

            // Arrange
            DeleteMealDto? deleteMealDto = new()
            {
                Id = 1,
                UserCompanyId = 1
            };

            // Act
            A.CallTo(() => _mealRepository.DetailMealAsync(deleteMealDto.Id)).Returns(meal);
            var result = await _mealService.DeleteMealAsync(deleteMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor não encontrado",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteMeal_NullDeleteMealDto_ReturnsError()
        {
            // Arrange
            DeleteMealDto? deleteMealDto = null;

            // Act
            var result = await _mealService.DeleteMealAsync(deleteMealDto);

            BaseResponse<GetMealDto> response = new()
            {
                Message = "Sabor não pode ser nulo",
                IsSuccess = false
            };

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }
    }
}