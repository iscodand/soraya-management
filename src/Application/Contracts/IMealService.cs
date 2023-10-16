using Application.Dtos.Meal;
using Application.Responses;
using Domain.Entities;

namespace Application.Contracts
{
    public interface IMealService
    {
        public Task<BaseResponse<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<BaseResponse<GetMealDto>> GetMealsByCompanyAsync(int companyId);
    }
}