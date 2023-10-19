namespace Application.Dtos.Meal
{
    public class UpdateMealDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public int UserCompanyId { get; set; }
    }
}