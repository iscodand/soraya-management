using Application.DTOs.Company.Requests;
using Application.Responses;

namespace Application.Contracts.Services
{
    public interface ICompanyService
    {
        public Task<BaseResponse<string>> CreateCompanyAsync(CreateCompanyRequest request);
    }
}