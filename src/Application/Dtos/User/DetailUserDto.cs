namespace Application.Dtos.User
{
    public class DetailUserDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Domain.Entities.Order> Orders { get; set; }
        public ICollection<Domain.Entities.Customer> Customers { get; set; }
        public ICollection<Domain.Entities.Meal> Meals { get; set; }
    }
}