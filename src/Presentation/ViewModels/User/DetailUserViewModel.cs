using Application.Dtos.Order;

namespace Presentation.ViewModels.User
{
    public class DetailUserViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string UserRole { get; set; }

        public ICollection<GetOrderDto> Orders { get; set; } = new List<GetOrderDto>();
    }
}