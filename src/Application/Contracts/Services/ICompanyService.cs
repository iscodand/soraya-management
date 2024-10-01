using Application.DTOs.Company.Requests;
using Application.DTOs.Company.Response;
using Application.Responses;

namespace Application.Contracts.Services
{
    public interface ICompanyService
    {
        public Task<BaseResponse<string>> CreateCompanyAsync(CreateCompanyRequest request);
        public Task<BaseResponse<IEnumerable<CompanyDTO>>> GetCompaniesAsync();
    }
}