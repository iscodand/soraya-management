using Application.DTOs.Company.Requests;
using Application.DTOs.Company.Response;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface ICompanyService
    {
        public Task<Response<string>> CreateCompanyAsync(CreateCompanyRequest request);
        public Task<Response<IEnumerable<CompanyDTO>>> GetCompaniesAsync();
<<<<<<< HEAD
=======
        public Task<Response<DetailCompanyDTO>> GetCompanyByIdAsync(int companyId);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
    }
}