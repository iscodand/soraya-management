using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.Authentication
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Seu nome de usuário é obrigatório.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Sua senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}