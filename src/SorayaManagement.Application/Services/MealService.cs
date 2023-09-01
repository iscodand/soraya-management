using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Meal;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Repositories;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Application.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<BaseResponse> CreateMealAsync(CreateMealDto createMealDto, User authenticatedUser)
        {
            if (createMealDto == null)
            {
                return new BaseResponse()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Meal meal = new()
            {
                Description = createMealDto.Description,
                Accompaniments = createMealDto.Accompaniments,
                CompanyId = authenticatedUser.CompanyId,
                UserId = authenticatedUser.Id,
            };

            await _mealRepository.CreateAsync(meal);

            return new BaseResponse()
            {
                Message = "Sabor salvo com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId)
        {
            if (companyId < 0)
            {
                return null;
            }

            ICollection<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);

            return meals;
        }
    }
}