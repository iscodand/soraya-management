using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTOs.Company.Requests;
using Application.Responses;
using Domain.Entities;

namespace Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // todo => adicionar validação para criação de nova empresa
        public async Task<BaseResponse<string>> CreateCompanyAsync(CreateCompanyRequest request)
        {
            bool companyAlreadyRegistered = await _companyRepository.CompanyAlreadyRegisteredByCNPJAsync(request.CNPJ);
            if (companyAlreadyRegistered)
            {
                return new BaseResponse<string>()
                {
                    IsSuccess = false,
                    Message = "Uma empresa já está cadastrada com esse CNPJ.",
                    Data = null
                };
            }

            Company company = new()
            {
                Name = request.Name,
                CNPJ = request.CNPJ
            };

            await _companyRepository.CreateAsync(company);

            return new BaseResponse<string>()
            {
                IsSuccess = true,
                Message = "Empresa cadastrada com sucesso.",
                Data = null
            };
        }
    }
}