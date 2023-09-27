using Microsoft.Identity.Client;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Order;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Data.Repositories;

namespace SorayaManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IMealRepository _mealRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderService(IOrderRepository orderRepository,
                            IPaymentTypeRepository paymentTypeRepository,
                            IMealRepository mealRepository,
                            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _mealRepository = mealRepository;
            _customerRepository = customerRepository;
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

        public async Task<BaseResponse<Order>> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null)
            {
                return new BaseResponse<Order>()
                {
                    Message = "Pedido não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Order order = await _orderRepository.GetByIdAsync(updateOrderDto.OrderId);

            if (order.CompanyId != updateOrderDto.CompanyId)
            {
                return new BaseResponse<Order>()
                {
                    Message = "Você não pode editar esse pedido. Ele não pertence a sua empresa.",
                    IsSuccess = false
                };
            }

            order.Description = updateOrderDto.Description;
            order.CustomerId = updateOrderDto.CustomerId;
            order.MealId = updateOrderDto.MealId;
            order.PaymentTypeId = updateOrderDto.PaymentTypeId;
            order.Price = updateOrderDto.Price;
            order.UpdatedAt = DateTime.Now;

            // Validate order payment
            if (order.IsPaid == true && updateOrderDto.IsPaid == false)
            {
                order.IsPaid = false;
                order.PaidAt = null;
            }
            else if (order.IsPaid == false && updateOrderDto.IsPaid == true)
            {
                order.IsPaid = true;
                order.PaidAt = DateTime.Now;
            }

            await _orderRepository.UpdateAsync(order);

            return new BaseResponse<Order>()
            {
                Message = $"Pedido N° {updateOrderDto.OrderId} atualizado com sucesso.",
                IsSuccess = true
            };
        }

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
            if (companyId <= 0)
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
            if (companyId <= 0)
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

        public async Task<BaseResponse<Order>> DeleteOrderAsync(int orderId, User authenticatedUser)
        {
            Order order = await _orderRepository.GetByIdAsync(orderId);

            if (authenticatedUser.CompanyId != order.CompanyId)
            {
                return new BaseResponse<Order>()
                {
                    Message = "Este pedido não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            await _orderRepository.DeleteAsync(order);

            return new BaseResponse<Order>()
            {
                Message = "Pedido deletado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId)
        {
            ICollection<Customer> customers = await _customerRepository.GetCustomersByCompanyAsync(companyId);
            ICollection<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);
            ICollection<PaymentType> paymentTypes = await _paymentTypeRepository.GetAllAsync();

            GetCreateOrderItemsDto orderItemsDto = new()
            {
                Customers = customers,
                Meals = meals,
                PaymentTypes = paymentTypes
            };

            return new BaseResponse<GetCreateOrderItemsDto>()
            {
                Message = "Sucesso.",
                IsSuccess = true,
                Data = orderItemsDto
            };
        }
    }
}