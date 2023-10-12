using Application.Contracts;
using Application.Dtos.Order;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.Repositories;

namespace Application.Services
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

        public async Task<BaseResponse<CreateOrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
            {
                return new BaseResponse<CreateOrderDto>()
                {
                    Message = "O Pedido não pode ser nulo.",
                    IsSuccess = false,
                };
            }

            Order order = Order.Create(
                createOrderDto.Description,
                createOrderDto.Price,
                createOrderDto.PaymentTypeId,
                createOrderDto.MealId,
                createOrderDto.CustomerId,
                createOrderDto.CompanyId,
                createOrderDto.UserId
            );

            await _orderRepository.CreateAsync(order);

            return new BaseResponse<CreateOrderDto>()
            {
                Message = "Pedido criado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<UpdateOrderDto>> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null)
            {
                return new BaseResponse<UpdateOrderDto>()
                {
                    Message = "Pedido não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Order order = await _orderRepository.GetByIdAsync(updateOrderDto.OrderId);

            if (order.CompanyId != updateOrderDto.CompanyId)
            {
                return new BaseResponse<UpdateOrderDto>()
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

            return new BaseResponse<UpdateOrderDto>()
            {
                Message = $"Pedido N° {updateOrderDto.OrderId} atualizado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<DetailOrderDto>> GetOrderDetailsAsync(int orderId, int userCompanyId)
        {
            Order order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (order == null)
            {
                return new BaseResponse<DetailOrderDto>()
                {
                    Message = "O pedido não foi encontrado.",
                    IsSuccess = false,
                };
            }

            if (order.CompanyId != userCompanyId)
            {
                return new BaseResponse<DetailOrderDto>()
                {
                    Message = "O pedido não foi encontrado.",
                    IsSuccess = false
                };
            }

            DetailOrderDto detailOrderDto = new()
            {
                Id = order.Id,
                Description = order.Description,
                Price = order.Price,
                IsPaid = order.IsPaid,
                PaidAt = order.PaidAt,
                PaymentType = order.PaymentType.Description,
                PaymentTypeId = order.PaymentType.Id,
                MealId = order.Meal.Id,
                Meal = order.Meal.Description,
                CustomerId = order.Customer.Id,
                Customer = order.Customer.Name,
                CreatedBy = order.User.Name,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };

            return new BaseResponse<DetailOrderDto>()
            {
                Data = detailOrderDto,
                Message = $"Pedido {detailOrderDto.Id} encontrado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetOrderDto>> GetOrdersByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new BaseResponse<GetOrderDto>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            ICollection<Order> orders = await _orderRepository.GetOrdersByCompanyAsync(companyId);

            List<GetOrderDto> getOrderDtoCollection = new();
            foreach (Order order in orders)
            {
                GetOrderDto getOrderDto = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType.Description,
                    Meal = order.Meal.Description,
                    Customer = order.Customer.Name,
                    CreatedBy = order.User.Name,
                    CreatedAt = order.CreatedAt
                };

                getOrderDtoCollection.Add(getOrderDto);
            }

            return new BaseResponse<GetOrderDto>()
            {
                DataCollection = getOrderDtoCollection,
                Message = "Pedidos encontrados com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetOrderDto>> GetOrdersByDateAsync(int companyId, DateTime? date)
        {
            if (companyId <= 0)
            {
                return new BaseResponse<GetOrderDto>()
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

            List<GetOrderDto> getOrderDtoCollection = new();
            foreach (Order order in orders)
            {
                GetOrderDto getOrderDto = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType.Description,
                    Meal = order.Meal.Description,
                    Customer = order.Customer.Name,
                    CreatedBy = order.User.Name,
                    CreatedAt = order.CreatedAt
                };

                getOrderDtoCollection.Add(getOrderDto);
            }

            return new BaseResponse<GetOrderDto>()
            {
                DataCollection = getOrderDtoCollection,
                Message = "Pedidos encontrados com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<UpdateOrderDto>> MakeOrderPaymentAsync(int orderId, int userCompanyId)
        {
            Order order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (order.CompanyId != userCompanyId)
            {
                return new BaseResponse<UpdateOrderDto>()
                {
                    Message = "Este pedido não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            order.MarkAsPaid(userCompanyId);
            await _orderRepository.UpdateAsync(order);

            return new BaseResponse<UpdateOrderDto>()
            {
                Message = "Pedido atualizado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetOrderDto>> DeleteOrderAsync(int orderId, int userCompanyId)
        {
            Order order = await _orderRepository.GetByIdAsync(orderId);

            if (order.CompanyId != userCompanyId)
            {
                return new BaseResponse<GetOrderDto>()
                {
                    Message = "Este pedido não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            await _orderRepository.DeleteAsync(order);

            return new BaseResponse<GetOrderDto>()
            {
                Message = "Pedido deletado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId)
        {
            ICollection<Customer> customers = await _customerRepository.GetActiveCustomersByCompanyAsync(companyId);
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