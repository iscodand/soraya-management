using Domain.Entities;

namespace Application.DTOs.Authentication
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public int CompanyId { get; set; }
        public string Role { get; set; }
    }
}