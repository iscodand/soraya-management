using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs.Authentication
{
    public class ChangePasswordDto
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Insira sua senha atual.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Insira sua nova senha.")]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Suas senhas não coincidem.")]
        public string ConfirmNewPassword { get; set; }
    }
}