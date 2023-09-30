using Application.Dtos.Customer;
using Application.Responses;
using Domain.Entities;

namespace Application.Contracts
{
    public interface ICustomerService
    {
        public Task<BaseResponse<Customer>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        public Task<BaseResponse<Customer>> GetCustomersByCompanyAsync(int companyId);
        public Task<BaseResponse<Customer>> DetailCustomerAsync(int customerId, User authenticatedUser);
    }
}