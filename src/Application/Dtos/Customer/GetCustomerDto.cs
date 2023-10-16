using Application.Dtos.User;

namespace Application.Dtos.Customer
{
    public class GetCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}