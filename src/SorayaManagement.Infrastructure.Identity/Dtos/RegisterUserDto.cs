using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Infrastructure.Identity.Dtos
{
    public class RegisterUserDto
    {
        [StringLength(125, MinimumLength = 5)]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [StringLength(32, MinimumLength = 8)]
        public string ConfirmPassword { get; set; }
    }
}