using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Order;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;

namespace SorayaManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;

        public OrderService(IOrderRepository orderRepository,
                            IPaymentTypeRepository paymentTypeRepository)
        {
            _orderRepository = orderRepository;
            _paymentTypeRepository = paymentTypeRepository;
        }

        public async Task<BaseResponse<Order>> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
            {
                return new BaseResponse<Order>()
                {
                    Message = "O Pedido não pode ser nulo.",
                    IsSuccess = false,
                };
            }

            // todo => add validation rules
            Order order = new()
            {
                Description = createOrderDto.Description,
                Price = createOrderDto.Price,
                IsPaid = false,
                PaidAt = null,
                PaymentTypeId = createOrderDto.PaymentTypeId,
                MealId = createOrderDto.MealId,
                CustomerId = createOrderDto.CustomerId,
                CompanyId = createOrderDto.CompanyId,
                UserId = createOrderDto.UserId
            };

            await _orderRepository.CreateAsync(order);

            return new BaseResponse<Order>()
            {
                Message = "Pedido criado com sucesso.",
                IsSuccess = true
            };
        }

        // todo => make a BaseResponse who returns Generic type (improve information result)
        public async Task<BaseResponse<Order>> GetOrderDetailsAsync(int orderId, User authenticatedUser)
        {
            Order order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (order == null)
            {
                return new BaseResponse<Order>()
                {
                    Message = "O pedido não foi encontrado.",
                    IsSuccess = false,
                };
            }

            if (authenticatedUser.CompanyId != order.CompanyId)
            {
                return new BaseResponse<Order>()
                {
                    Message = "O pedido não foi encontrado.",
                    IsSuccess = false
                };
            }

            return new BaseResponse<Order>()
            {
                Data = order,
                Message = $"Pedido {order.Id} encontrado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<Order>> GetOrdersByCompanyAsync(int companyId)
        {
            if (companyId < 0)
            {
                return new BaseResponse<Order>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            ICollection<Order> orders = await _orderRepository.GetOrdersByCompanyAsync(companyId);

            return new BaseResponse<Order>()
            {
                DataCollection = orders,
                Message = "Pedidos encontrados com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<Order>> GetOrdersByDateAsync(int companyId, DateTime? date)
        {
            if (companyId < 0)
            {
                return new BaseResponse<Order>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            if (date == null)
            {
                // Date never goes null to repository
                date = DateTime.Today.Date;
            }

            ICollection<Order> orders = await _orderRepository.GetOrdersByDateAsync(companyId, date);

            return new BaseResponse<Order>()
            {
                DataCollection = orders,
                Message = "Pedidos encontrados com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<PaymentType>> GetPaymentTypesAsync()
        {
            ICollection<PaymentType> paymentTypes = await _paymentTypeRepository.GetAllAsync();

            return new BaseResponse<PaymentType>()
            {
                Message = "Tipos de Pagamento encontrados com sucesso",
                IsSuccess = true,
                DataCollection = paymentTypes
            };
        }

        public async Task<BaseResponse<Order>> MakeOrderPaymentAsync(int orderId, User authenticatedUser)
        {
            Order order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (authenticatedUser.CompanyId != order.CompanyId)
            {
                return new BaseResponse<Order>()
                {
                    Message = "Este pedido não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            order.IsPaid = true;
            order.PaidAt = DateTime.Now;

            await _orderRepository.UpdateAsync(order);

            return new BaseResponse<Order>()
            {
                Message = "Pedido atualizado com sucesso.",
                IsSuccess = true
            };
        }
    }
}