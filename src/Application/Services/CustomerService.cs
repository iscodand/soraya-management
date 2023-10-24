using Application.Contracts;
using Application.Dtos.Customer;
using Application.Dtos.Order;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Data.Contracts;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<BaseResponse<CreateCustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                return new BaseResponse<CreateCustomerDto>()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            if (await _customerRepository.CustomerExistsByCompanyAsync(createCustomerDto.Name, createCustomerDto.CompanyId))
            {
                return new BaseResponse<CreateCustomerDto>()
                {
                    Message = "Um cliente com esse nome já foi cadastrado. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            Customer customer = Customer.Create(
                createCustomerDto.Name,
                createCustomerDto.Phone,
                createCustomerDto.CompanyId,
                createCustomerDto.UserId
            );

            await _customerRepository.CreateAsync(customer);

            return new BaseResponse<CreateCustomerDto>()
            {
                Message = "Cliente criado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<UpdateCustomerDto>> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            if (updateCustomerDto == null)
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            if (await _customerRepository.CustomerExistsByCompanyAsync(updateCustomerDto.Name, updateCustomerDto.UserCompanyId))
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Um cliente com esse nome já foi cadastrado. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            Customer customer = await _customerRepository.GetByIdAsync(updateCustomerDto.Id);

            if (customer.CompanyId != updateCustomerDto.UserCompanyId)
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            customer.Update(
                updateCustomerDto.Name,
                updateCustomerDto.Phone
            );

            await _customerRepository.UpdateAsync(customer);

            return new BaseResponse<UpdateCustomerDto>()
            {
                Message = "Cliente atualizado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetCustomerDto>> GetCustomerByIdAsync(int customerId, int userCompanyId)
        {
            if (userCompanyId <= 0)
            {
                return new BaseResponse<GetCustomerDto>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            Customer customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (userCompanyId != customer.CompanyId)
            {
                return new BaseResponse<GetCustomerDto>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            GetCustomerDto getCustomerDto = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                IsActive = customer.IsActive,
                CreatedBy = customer.User.Name
            };

            return new BaseResponse<GetCustomerDto>()
            {
                Message = "Cliente encontrado com sucesso.",
                IsSuccess = true,
                Data = getCustomerDto
            };
        }


        public async Task<BaseResponse<GetCustomerDto>> GetCustomersByCompanyAsync(int userCompanyId)
        {
            if (userCompanyId <= 0)
            {
                return new BaseResponse<GetCustomerDto>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            ICollection<Customer> customers = await _customerRepository.GetCustomersByCompanyAsync(userCompanyId);
            ICollection<GetCustomerDto> getCustomerDtoCollection = new List<GetCustomerDto>();
            foreach (Customer customer in customers)
            {
                GetCustomerDto getCustomerDto = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    IsActive = customer.IsActive,
                    CreatedBy = customer.User.Name,
                    OrdersCount = customer.Orders.Count
                };

                getCustomerDtoCollection.Add(getCustomerDto);
            }

            return new BaseResponse<GetCustomerDto>()
            {
                Message = "Clientes encontrados com sucesso.",
                IsSuccess = true,
                DataCollection = getCustomerDtoCollection
            };
        }

        public async Task<BaseResponse<DetailCustomerDto>> DetailCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            // iscodand - 16/10/23 => Filtering orders by current month
            ICollection<Order> customerOrders = customer.Orders.Where(x => x.CreatedAt.Month == DateTime.Now.Month).ToList();

            List<GetOrderDto> getOrderDtoCollection = new();
            foreach (Order order in customerOrders)
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
                    CreatedAt = order.CreatedAt
                };

                getOrderDtoCollection.Add(getOrderDto);
            }

            DetailCustomerDto detailCustomerDto = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                IsActive = customer.IsActive,
                CreatedBy = customer.User.Name,
                Orders = getOrderDtoCollection
            };

            if (userCompanyId != customer.CompanyId)
            {
                return new BaseResponse<DetailCustomerDto>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            return new BaseResponse<DetailCustomerDto>()
            {
                Message = "Cliente encontrado com sucesso",
                IsSuccess = true,
                Data = detailCustomerDto
            };
        }

        public async Task<BaseResponse<UpdateCustomerDto>> InactivateCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (!customer.IsActive)
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Cliente já está inativo",
                    IsSuccess = false
                };
            }

            if (customer.CompanyId != userCompanyId)
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Você não pode desativar clientes de outras empresas.",
                    IsSuccess = false
                };
            }

            customer.Inactivate();

            await _customerRepository.UpdateAsync(customer);

            return new BaseResponse<UpdateCustomerDto>()
            {
                Message = "Cliente desativado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<UpdateCustomerDto>> ActivateCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer.IsActive)
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Cliente já está ativo",
                    IsSuccess = false
                };
            }

            if (customer.CompanyId != userCompanyId)
            {
                return new BaseResponse<UpdateCustomerDto>()
                {
                    Message = "Você não pode ativar clientes de outras empresas.",
                    IsSuccess = false
                };
            }

            customer.Activate();

            await _customerRepository.UpdateAsync(customer);

            return new BaseResponse<UpdateCustomerDto>()
            {
                Message = "Cliente ativado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetCustomerDto>> DeleteCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            if (customer.CompanyId != userCompanyId)
            {
                return new BaseResponse<GetCustomerDto>()
                {
                    Message = "Você não pode excluir clientes de outras empresas.",
                    IsSuccess = false
                };
            }

            if (customer.Orders.Any())
            {
                return new BaseResponse<GetCustomerDto>()
                {
                    Message = "Você não pode excluir esse cliente pois ele possui pedidos cadastrados.",
                    IsSuccess = false
                };
            }

            await _customerRepository.DeleteAsync(customer);

            return new BaseResponse<GetCustomerDto>()
            {
                Message = "Cliente deletado com sucesso.",
                IsSuccess = true
            };
        }
    }
}