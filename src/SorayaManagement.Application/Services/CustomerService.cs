using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Customer;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;

namespace SorayaManagement.Application.Services
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

            Customer newCustomer = new()
            {
                Name = createCustomerDto.Name,
                UserId = createCustomerDto.UserId,
                CompanyId = createCustomerDto.CompanyId
            };

            await _customerRepository.CreateAsync(newCustomer);

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

        public async Task<BaseResponse<Customer>> DetailCustomerAsync(int customerId, User authenticatedUser)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            if (authenticatedUser.CompanyId != customer.CompanyId)
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
    }
}