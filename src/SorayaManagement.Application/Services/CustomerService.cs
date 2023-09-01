using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Customer;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<BaseResponse> CreateCustomerAsync(CreateCustomerDto createCustomerDto, User authenticatedUser)
        {
            if (createCustomerDto == null)
            {
                return new BaseResponse()
                {
                    Message = "Cliente não pode ser nulo.",
                    IsSuccess = false
                };
            }

            Customer newCustomer = new()
            {
                Name = createCustomerDto.Name,
                UserId = authenticatedUser.Id,
                CompanyId = authenticatedUser.CompanyId
            };

            await _customerRepository.CreateAsync(newCustomer);

            return new BaseResponse()
            {
                Message = "Cliente criado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<ICollection<Customer>> GetCustomersByCompanyAsync(int companyId)
        {
            if (companyId < 0)
            {
                return null;
            }

            ICollection<Customer> customers = await _customerRepository.GetCustomersByCompanyAsync(companyId);

            return customers;
        }
    }
}