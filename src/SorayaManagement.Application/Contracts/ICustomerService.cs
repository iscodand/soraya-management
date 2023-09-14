using SorayaManagement.Application.Dtos.Customer;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Application.Contracts
{
    public interface ICustomerService
    {
        public Task<BaseResponse<Customer>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        public Task<BaseResponse<Customer>> GetCustomersByCompanyAsync(int companyId);
    }
}