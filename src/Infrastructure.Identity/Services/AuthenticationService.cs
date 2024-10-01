using Application.Contracts.Services;
using Application.DTOs.Authentication;
using Application.DTOs.Email;
using Application.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AuthenticationService(UserManager<User> userManager,
                                     SignInManager<User> signInManager,
                                     IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<BaseResponse<string>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
            {
                return new()
                {
                    Message = "Usuário não pode ser nulo.",
                    IsSuccess = false
                };
            }

            User user = User.Create(
                registerUserDto.Name,
                registerUserDto.Email,
                registerUserDto.Username,
                registerUserDto.CompanyId
            );

            IdentityResult result = await _userManager.CreateAsync(user, registerUserDto.Password);

            // Add user to specified Role
            await _userManager.AddToRoleAsync(user, registerUserDto.Role);

            // Verify duplicate e-mail and username
            Dictionary<string, string> identityErrorMapping = new()
            {
                { "DuplicateUserName", "Esse nome de usuário já está em uso." },
                { "DuplicateEmail", "Esse e-mail já está em uso." }
            };

            if (!result.Succeeded)
            {
                if (identityErrorMapping.TryGetValue(result.Errors.FirstOrDefault().Code, out string errorDescription))
                {
                    return new()
                    {
                        Message = errorDescription,
                        IsSuccess = false
                    };
                }
            }

            return new()
            {
                Message = "Usuário cadastrado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<string>> LoginAsync(LoginUserDto loginUserDto)
        {
            User user = await _userManager.FindByNameAsync(loginUserDto.UserName);

            if (user == null)
            {
                return new()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            if (!user.IsActive)
            {
                return new()
                {
                    Message = "Usuário está inativo no sistema. Consulte seu gestor e tente novamente.",
                    IsSuccess = false
                };
            }

            SignInResult signIn = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password!,
                                                                           isPersistent: false, lockoutOnFailure: false);
            if (!signIn.Succeeded)
            {
                return new()
                {
                    Message = "Senha inválida. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            return new()
            {
                Message = "Login realizado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<string>> ForgotPasswordAsync(string email, string origin)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new()
                {
                    Message = "Esse e-mail não está cadastrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            // Isco 24/11/2023
            // Gerando token para recuperação de senha (após gerar o token com o Identity, ele é criptografado para aumentar a segurança)
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] encodingToken = Encoding.UTF8.GetBytes(token);
            string validToken = WebEncoders.Base64UrlEncode(encodingToken);

            string url = $"{origin}/nova-senha?email={user.Email}&token={validToken}";

            SendMailRequest sendMailRequest = new()
            {
                To = user.Email,
                Subject = "Recuperar Senha",
                Body = url
                // TemplatePath = "ForgotPasswordTemplate.html",
                // Parameters = {
                //     { "user.FullName", user.Name },
                //     { "url", url }
                // }
            };

            await _emailService.SendMailAsync(sendMailRequest);

            return new()
            {
                Message = "Foi enviado um link de recuperação para o seu e-mail.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            User user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user is null)
            {
                return new()
                {
                    Message = "Esse e-mail não está cadastrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            bool passwordAlreadyRegistered = await _userManager.CheckPasswordAsync(user, resetPasswordDto.NewPassword);
            if (passwordAlreadyRegistered)
            {
                return new()
                {
                    Message = "Sua nova senha não deve ser igual à anterior.",
                    IsSuccess = false
                };
            }

            // Descriptografando o token para conseguir usá-lo na recuperação de senha
            byte[] decodedToken = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, normalToken, resetPasswordDto.NewPassword);

            // Verify duplicate e-mail and username
            Dictionary<string, string> identityErrorMapping = new()
            {
                { "InvalidToken", "Esse link não é mais válido. Solicite outra alteração ou entre em contato com o suporte." },
            };

            if (!result.Succeeded)
            {
                if (identityErrorMapping.TryGetValue(result.Errors.FirstOrDefault().Code, out string errorDescription))
                {
                    return new()
                    {
                        Message = errorDescription,
                        IsSuccess = false
                    };
                }
            }

            return new()
            {
                Message = "Sua senha foi resetada com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<string>> ChangePasswordAsync(string username, ChangePasswordDto changePasswordDto)
        {
            User user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                return new()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            bool checkOldPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword);
            if (checkOldPassword == false)
            {
                return new()
                {
                    Message = "Sua senha antiga está incorreta.",
                    IsSuccess = false
                };
            }

            //bool passwordAlreadyInUse = await _userManager.CheckPasswordAsync(user, changePasswordDto.NewPassword);
            //if (passwordAlreadyInUse)
            //{
            //    return new()
            //    {
            //        Message = "Sua nova senha não pode ser igual à anterior.",
            //        IsSuccess = false
            //    };
            //}

            IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new()
                {
                    Message = "Ops. Ocorreu um erro ao alterar sua senha, verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            return new()
            {
                Message = "Senha alterada com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<string>> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return new()
            {
                Message = "Logout realizado com sucesso.",
                IsSuccess = true
            };
        }
    }
}