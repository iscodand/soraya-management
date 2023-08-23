using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Infrastructure.Identity.Dtos
{
    public class LoginUserDto
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }
    }
}