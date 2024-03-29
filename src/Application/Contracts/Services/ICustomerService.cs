using Application.Dtos.Customer;
using Application.Responses;

namespace Application.Contracts.Services
{
    public interface ICustomerService
    {
        public Task<BaseResponse<CreateCustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        public Task<BaseResponse<UpdateCustomerDto>> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        public Task<BaseResponse<UpdateCustomerDto>> ActivateCustomerAsync(int customerId, int userCompanyId);
        public Task<BaseResponse<UpdateCustomerDto>> InactivateCustomerAsync(int customerId, int userCompanyId);
        public Task<BaseResponse<GetCustomerDto>> DeleteCustomerAsync(int customerId, int userCompanyId);
        public Task<BaseResponse<GetCustomerDto>> GetCustomersByCompanyAsync(int userCompanyId);
        public Task<BaseResponse<GetCustomerDto>> GetCustomersByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
        public Task<BaseResponse<GetCustomerDto>> GetCustomerByIdAsync(int customerId, int userCompanyId);
        public Task<BaseResponse<DetailCustomerDto>> DetailCustomerAsync(int customerId, int userCompanyId);
    }
}