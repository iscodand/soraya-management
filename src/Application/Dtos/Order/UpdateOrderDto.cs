namespace Application.Dtos.Order
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }

        public string Description { get; set; }
        public int CustomerId { get; set; }
        public int MealId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }

        public int CompanyId { get; set; }
        public string UserId { get; set; }
    }
}