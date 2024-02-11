using Application.Contracts.Services;
using Application.DTOs.Company.Requests;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultCompany
    {
        public static async Task SeedAsync(ICompanyService companyService)
        {
            CreateCompanyRequest request = new()
            {
                Name = "Delícias da Soraya",
                CNPJ = "12345678900000"
            };

            await companyService.CreateCompanyAsync(request);
        }
    }
}