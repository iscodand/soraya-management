namespace ViewModels
{
    public class CreateOrderViewModel
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PaymentTypeId { get; set; }
        public int MealId { get; set; }
        public int CustomerId { get; set; }
        public CreateOrderDropdown CreateOrderDropdown { get; set; }
    }
}