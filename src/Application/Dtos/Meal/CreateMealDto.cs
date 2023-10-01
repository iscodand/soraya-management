namespace Application.Dtos.Meal
{
    public class CreateMealDto
    {
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}