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

        public async Task<BaseResponse<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto)
        {
            if (createMealDto == null)
            {
                return new BaseResponse<CreateMealDto>()
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

            return new BaseResponse<CreateMealDto>()
            {
                Message = "Sabor salvo com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetMealDto>> GetMealsByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new BaseResponse<GetMealDto>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            ICollection<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);

            List<GetMealDto> getMealDtoCollection = new();
            foreach (Meal meal in meals)
            {
                GetMealDto getMealDto = new()
                {
                    Id = meal.Id,
                    Description = meal.Description,
                    Accompaniments = meal.Accompaniments,
                    CreatedBy = meal.User.Name
                };

                getMealDtoCollection.Add(getMealDto);
            }

            return new BaseResponse<GetMealDto>()
            {
                Message = "Sabores encontrados com sucesso",
                IsSuccess = true,
                DataCollection = getMealDtoCollection
            };
        }
    }
}