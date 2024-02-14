namespace Application.DTOs.Company.Requests
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }

        public static Domain.Entities.Company Map(Domain.Entities.Company company, CreateCompanyRequest request)
        {
            company.Name = request.Name;
            company.CNPJ = request.CNPJ;

            return company;
        }
    }
}