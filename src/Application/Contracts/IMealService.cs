using SorayaManagement.Application.Dtos.Meal;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Application.Contracts
{
    public interface IMealService
    {
        public Task<BaseResponse<Meal>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<BaseResponse<Meal>> GetMealsByCompanyAsync(int companyId);
    }
}