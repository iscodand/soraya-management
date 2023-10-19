namespace Presentation.ViewModels.Meal
{
    public class UpdateMealViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public int UserCompanyId { get; set; }
    }
}