namespace Presentation.ViewModels.Customer
{
    public class DetailCustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Domain.Entities.Order> Orders { get; set; }
    }
}