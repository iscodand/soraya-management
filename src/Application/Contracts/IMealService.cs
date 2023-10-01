using Application.Dtos.Meal;
using Application.Responses;
using Domain.Entities;

namespace Application.Contracts
{
    public interface IMealService
    {
        public Task<BaseResponse<Meal>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<BaseResponse<Meal>> GetMealsByCompanyAsync(int companyId);
    }
}