using Application.Dtos.Customer;
<<<<<<< HEAD
=======
using Application.Parameters;
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface ICustomerService
    {
<<<<<<< HEAD
        public Task<Response<IEnumerable<GetCustomerDto>>> GetCustomersByCompanyAsync(int userCompanyId);
=======
        public Task<PagedResponse<IEnumerable<GetCustomerDto>>> GetCustomersByCompanyAsync(int companyId, RequestParameter parameter);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        public Task<Response<IEnumerable<GetCustomerDto>>> GetCustomersByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
        public Task<Response<GetCustomerDto>> GetCustomerByIdAsync(int customerId, int userCompanyId);
        public Task<Response<DetailCustomerDto>> DetailCustomerAsync(int customerId, int userCompanyId);
        public Task<Response<CreateCustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        public Task<Response<UpdateCustomerDto>> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        public Task<Response<UpdateCustomerDto>> ActivateCustomerAsync(int customerId, int userCompanyId);
        public Task<Response<UpdateCustomerDto>> InactivateCustomerAsync(int customerId, int userCompanyId);
        public Task<Response<GetCustomerDto>> DeleteCustomerAsync(int customerId, int userCompanyId);
    }
}