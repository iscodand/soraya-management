namespace Presentation.ViewModels.Meal
{
    public class GetMealViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string CreatedBy { get; set; }
        public int OrdersCount { get; set; }
    }
}