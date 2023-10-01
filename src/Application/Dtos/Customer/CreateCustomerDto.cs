namespace Application.Dtos.Customer
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}