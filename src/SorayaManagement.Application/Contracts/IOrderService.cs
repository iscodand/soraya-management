using SorayaManagement.Application.Dtos.Order;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Application.Contracts
{
    public interface IOrderService
    {
        public Task<BaseResponse<Order>> CreateOrderAsync(CreateOrderDto createOrderDto, User authenticatedUser);
        public Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId);
        public Task<BaseResponse<Order>> GetOrderDetailsAsync(int orderId, User authenticatedUser);
        public Task<BaseResponse<Order>> MakeOrderPaymentAsync(int orderId, User authenticatedUser);
        public Task<ICollection<PaymentType>> GetPaymentTypesAsync();
    }
}