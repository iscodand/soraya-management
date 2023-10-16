namespace Application.Dtos.Customer
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int UserCompanyId { get; set; }
    }
}