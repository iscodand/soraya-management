using Application.Dtos.Order;

namespace Application.Dtos.Customer
{
    public class DetailCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<GetOrderDto> Orders { get; set; }
    }
}