using Presentation.ViewModels.Order;

namespace Presentation.ViewModels.Customer
{
    public class DetailCustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<GetOrderViewModel> Orders { get; set; }
    }
}