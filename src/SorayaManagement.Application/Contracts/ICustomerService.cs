// todo => pull apart BaseResponse
using SorayaManagement.Application.Dtos.Customer;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Application.Contracts
{
    public interface ICustomerService
    {
        public Task<BaseResponse> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        public Task<ICollection<Customer>> GetCustomersByCompanyAsync(int companyId);
    }
}