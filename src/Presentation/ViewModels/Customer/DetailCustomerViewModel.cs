using Domain.Entities;

namespace ViewModels
{
    public class DetailCustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}