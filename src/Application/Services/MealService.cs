using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Meal;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Repositories;

namespace SorayaManagement.Application.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<BaseResponse<Meal>> CreateMealAsync(CreateMealDto createMealDto)
        {
            if (createMealDto == null)
            {
                return new BaseResponse<Meal>()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Meal meal = new()
            {
                Description = createMealDto.Description,
                Accompaniments = createMealDto.Accompaniments,
                CompanyId = createMealDto.CompanyId,
                UserId = createMealDto.UserId
            };

            await _mealRepository.CreateAsync(meal);

            return new BaseResponse<Meal>()
            {
                Message = "Sabor salvo com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<Meal>> GetMealsByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new BaseResponse<Meal>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            ICollection<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);

            return new BaseResponse<Meal>()
            {
                Message = "Sabores encontrados com sucesso",
                IsSuccess = true,
                DataCollection = meals
            };
        }
    }
}