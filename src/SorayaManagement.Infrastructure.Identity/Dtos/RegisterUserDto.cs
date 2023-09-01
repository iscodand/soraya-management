using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Infrastructure.Identity.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Insira seu nome.")]
        [StringLength(125, MinimumLength = 5, ErrorMessage = "Seu nome precisa ter no mínimo 5 caracteres e no máximo 128.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Seu nome de usuário é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Sua senha precisa ter no mínimo 3 caracteres e no máximo 100.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Seu E-mail é obrigatório.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Sua senha é obrigatória.")]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Sua senha precisa ter no mínimo 8 caracteres e no máximo 32.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Suas senhas não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}