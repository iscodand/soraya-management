using SorayaManagement.Application.Dtos.Order;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Application.Contracts
{
    public interface IOrderService
    {
        public Task<BaseResponse<Order>> GetOrdersByCompanyAsync(int companyId);
        public Task<BaseResponse<Order>> GetOrdersByDateAsync(int companyId, DateTime? date);
        public Task<BaseResponse<Order>> GetOrderDetailsAsync(int orderId, User authenticatedUser);
        public Task<BaseResponse<Order>> CreateOrderAsync(CreateOrderDto createOrderDto);
        public Task<BaseResponse<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId);
        public Task<BaseResponse<Order>> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        public Task<BaseResponse<Order>> MakeOrderPaymentAsync(int orderId, User authenticatedUser);
        public Task<BaseResponse<Order>> DeleteOrderAsync(int orderId, User authenticatedUser);
    }
}