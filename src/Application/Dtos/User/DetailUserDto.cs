using Application.Dtos.Order;
using Microsoft.Extensions.Primitives;

namespace Application.Dtos.User
{
    public class DetailUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }

        public IEnumerable<GetOrderDto> Orders { get; set; }

        //public ICollection<Domain.Entities.Customer> Customers { get; set; }
        //public ICollection<Domain.Entities.Meal> Meals { get; set; }

        public static DetailUserDto Map(Domain.Entities.User user, string role)
        {
            return new DetailUserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                Role = role,
                Orders = GetOrderDto.Map(user.Orders)
            };
        }
    }
}