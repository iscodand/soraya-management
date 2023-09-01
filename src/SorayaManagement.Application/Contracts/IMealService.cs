using SorayaManagement.Application.Dtos.Meal;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Application.Contracts
{
    public interface IMealService
    {
        public Task<BaseResponse> CreateMealAsync(CreateMealDto createMealDto, User authenticatedUser);
        public Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId);
    }
}