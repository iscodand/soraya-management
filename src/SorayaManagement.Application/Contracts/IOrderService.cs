using SorayaManagement.Application.Dtos;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Application.Contracts
{
    public interface IOrderService
    {
        public Task<BaseResponse> CreateOrderAsync(CreateOrderDto createOrderDto, User authenticatedUser);
        public Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId);
    }
}