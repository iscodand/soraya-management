using System.ComponentModel.DataAnnotations;
using Application.DTOs.Authentication;

namespace Presentation.ViewModels.Authentication
{
    public class ResetPasswordViewModel
    {
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Sua senha precisa conter entre 8 e 32 caracteres.")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Suas senhas não coincidem.")]
        public string ConfirmPassword { get; set; }

        public static ResetPasswordDto MapToDto(ResetPasswordViewModel viewModel, string token, string email)
        {
            return new ResetPasswordDto()
            {
                Email = email,
                Token = token,
                NewPassword = viewModel.Password,
                ConfirmNewPassword = viewModel.ConfirmPassword
            };
        }
    }
}