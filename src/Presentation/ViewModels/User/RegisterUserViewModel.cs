using System.ComponentModel.DataAnnotations;
using Application.Dtos.User;

namespace UI.ViewModels.User.Employee
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Insira seu nome.")]
        [StringLength(125, MinimumLength = 5, ErrorMessage = "Seu nome precisa ter no m�nimo 5 caracteres e no m�ximo 128.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Seu nome de usu�rio � obrigat�rio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Sua senha precisa ter no m�nimo 3 caracteres e no m�ximo 100.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Seu E-mail � obrigat�rio.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Sua senha � obrigat�ria.")]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Sua senha precisa ter no m�nimo 8 caracteres e no m�ximo 32.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Suas senhas n�o coincidem.")]
        public string ConfirmPassword { get; set; }

        public string RoleId { get; set; }

        public ICollection<GetRolesDto> RolesDropdown { get; set; }
    }
}