using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Infrastructure.Identity.Dtos
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Seu nome de usuário é obrigatório.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Sua senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}