using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<BaseResponse> CreateOrderAsync(CreateOrderDto createOrderDto, User authenticatedUser)
        {
            if (createOrderDto == null)
            {
                return new BaseResponse()
                {
                    Message = "O Pedido não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Order order = new()
            {
                Description = createOrderDto.Description,
                Price = createOrderDto.Price,
                IsPaid = createOrderDto.IsPaid,
                PaidAt = createOrderDto.PaidAt,
                PaymentTypeId = createOrderDto.PaymentTypeId,
                MealId = createOrderDto.MealId,
                CustomerId = createOrderDto.CustomerId,

                CompanyId = authenticatedUser.CompanyId,
                UserId = authenticatedUser.Id
            };

            await _orderRepository.CreateAsync(order);

            return new BaseResponse()
            {
                Message = "Pedido criado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId)
        {
            if (companyId < 0)
            {
                return null;
            }

            ICollection<Order> orders = await _orderRepository.GetOrdersByCompanyAsync(companyId);

            return orders;
        }
    }
}