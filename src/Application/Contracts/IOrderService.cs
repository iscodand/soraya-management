using Application.Dtos.Order;
using Application.Responses;
using Domain.Entities;

namespace Application.Contracts
{
    public interface IOrderService
    {
        public Task<BaseResponse<GetOrderDto>> GetOrdersByCompanyAsync(int companyId);
        public Task<BaseResponse<GetOrderDto>> GetOrdersByDateAsync(int companyId, DateTime? date);
        public Task<BaseResponse<DetailOrderDto>> GetOrderDetailsAsync(int orderId, int userCompanyId);
        public Task<BaseResponse<CreateOrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
        public Task<BaseResponse<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId);
        public Task<BaseResponse<UpdateOrderDto>> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        public Task<BaseResponse<UpdateOrderDto>> MakeOrderPaymentAsync(int orderId, int userCompanyId);
        public Task<BaseResponse<GetOrderDto>> DeleteOrderAsync(int orderId, int userCompanyId);
    }
}