namespace SorayaManagement.ViewModels
{
    public class GetOrderViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaymentType { get; set; }
        public string Meal { get; set; }
        public string Customer { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}