namespace Application.Dtos.Meal
{
    public class GetMealDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string CreatedBy { get; set; }
    }
}