namespace Presentation.ViewModels.Order
{
    public class UpdateOrderViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CustomerId { get; set; }
        public int MealId { get; set; }
        public int PaymentTypeId { get; set; }

        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public CreateOrderDropdown CreateOrderDropdown { get; set; }
    }
}