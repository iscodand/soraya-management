namespace Application.Dtos.Order
{
    public class CreateOrderDto
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MealId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentTypeId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; }
    }
}