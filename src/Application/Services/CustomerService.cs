using Application.Contracts;
using Application.Dtos.Customer;
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

        public async Task<BaseResponse<Customer>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            if (await _customerRepository.CustomerExistsByNameAsync(createCustomerDto.Name))
            {
                return new BaseResponse<Customer>()
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

            return new BaseResponse<Customer>()
            {
                Message = "Cliente criado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<Customer>> GetCustomersByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            ICollection<Customer> customers = await _customerRepository.GetCustomersByCompanyAsync(companyId);

            return new BaseResponse<Customer>()
            {
                Message = "Clientes encontrados com sucesso.",
                IsSuccess = true,
                DataCollection = customers
            };
        }

        public async Task<BaseResponse<Customer>> DetailCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            if (userCompanyId != customer.CompanyId)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            return new BaseResponse<Customer>()
            {
                Message = "Cliente encontrado com sucesso",
                IsSuccess = true,
                Data = customer
            };
        }

        public async Task<BaseResponse<Customer>> InactivateCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (!customer.IsActive)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Cliente já está inativo",
                    IsSuccess = false
                };
            }

            if (customer.CompanyId != userCompanyId)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Você não pode desativar clientes de outras empresas.",
                    IsSuccess = false
                };
            }

            customer.Inactivate();

            await _customerRepository.UpdateAsync(customer);

            return new BaseResponse<Customer>()
            {
                Message = "Cliente desativado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<Customer>> ActivateCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer.IsActive)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Cliente já está ativo",
                    IsSuccess = false
                };
            }

            if (customer.CompanyId != userCompanyId)
            {
                return new BaseResponse<Customer>()
                {
                    Message = "Você não pode ativar clientes de outras empresas.",
                    IsSuccess = false
                };
            }

            customer.Activate();

            await _customerRepository.UpdateAsync(customer);

            return new BaseResponse<Customer>()
            {
                Message = "Cliente ativado com sucesso",
                IsSuccess = true
            };
        }
    }
}