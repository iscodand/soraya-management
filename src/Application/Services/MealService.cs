using Application.Contracts;
using Application.Dtos.Meal;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Data.Repositories;

namespace Application.Services
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

            Meal meal = Meal.Create(
                createMealDto.Description,
                createMealDto.Accompaniments,
                createMealDto.CompanyId,
                createMealDto.UserId
            );

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