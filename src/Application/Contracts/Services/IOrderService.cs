using Application.Dtos.Order;
<<<<<<< HEAD
=======
using Application.Parameters;
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IOrderService
    {
        public Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByCompanyAsync(int companyId);
<<<<<<< HEAD
        public Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByDateAsync(int companyId, DateTime? date);
=======
        public Task<PagedResponse<IEnumerable<GetOrderDto>>> GetOrdersByDateAsync(int companyId, DateTime? date);
        public Task<PagedResponse<IEnumerable<GetOrderDto>>> GetOrdersByDateRangePagedAsync(int companyId, RequestParameter parameter);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        public Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByDateRangeAsync(int companyId, DateTime? initialDate, DateTime? finalDate);
        public Task<Response<DetailOrderDto>> GetOrderDetailsAsync(int orderId, int userCompanyId);
        public Task<Response<CreateOrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
        public Task<Response<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId);
        public Task<Response<UpdateOrderDto>> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        public Task<Response<UpdateOrderDto>> MakeOrderPaymentAsync(int orderId, int userCompanyId);
        public Task<Response<GetOrderDto>> DeleteOrderAsync(int orderId, int userCompanyId);
    }
}