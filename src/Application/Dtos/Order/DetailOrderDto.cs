namespace Application.Dtos.Order
{
    public class DetailOrderDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public int MealId { get; set; }
        public string Meal { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}