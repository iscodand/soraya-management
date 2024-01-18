using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Infrastructure.Identity.Contracts;
using Infrastructure.Identity.Dtos;
using Infrastructure.Identity.Responses;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<BaseResponse> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
            {
                return new BaseResponse()
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
                    return new BaseResponse()
                    {
                        Message = errorDescription,
                        IsSuccess = false
                    };
                }
            }

            return new BaseResponse()
            {
                Message = "Usuário cadastrado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse> LoginAsync(LoginUserDto loginUserDto)
        {
            User user = await _userManager.FindByNameAsync(loginUserDto.UserName);

            if (user == null)
            {
                return new BaseResponse()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente",
                    IsSuccess = false
                };
            }

            if (!user.IsActive)
            {
                return new BaseResponse()
                {
                    Message = "Usuário está inativo no sistema. Consulte seu gestor e tente novamente.",
                    IsSuccess = false
                };
            }

            SignInResult signIn = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password!,
                                                                           isPersistent: false, lockoutOnFailure: false);
            if (!signIn.Succeeded)
            {
                return new BaseResponse()
                {
                    Message = "Senha inválida. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            return new BaseResponse()
            {
                Message = "Login realizado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse> ForgotPasswordAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new BaseResponse()
                {
                    Message = "Usuário não encontrado.",
                    IsSuccess = false
                };
            }
            // Isco 24/11/2023
            // Gerando token para recuperação de senha (após gerar o token com o Identity, ele é criptografado para aumentar a segurança)
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] encodingToken = Encoding.UTF8.GetBytes(token);
            string validToken = WebEncoders.Base64UrlEncode(encodingToken);

            // string url = $"{origin}/auth/nova-senha?email={user.Email}&token={validToken}";

            // SendMailRequest sendMailRequest = new()
            // {
            //     To = user.Email,
            //     Subject = "Recuperar Senha - Seletivo ABREM",
            //     TemplatePath = "ForgotPasswordTemplate.html",
            //     Parameters = {
            //         { "user.FullName", user.Candidate.FullName },
            //         { "url", url }
            //     }
            // };

            // await _emailService.SendEmailAsync(sendMailRequest);

            return new BaseResponse()
            {
                Message = "Foi enviado um link de recuperação para o seu e-mail.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return new BaseResponse()
            {
                Message = "Logout realizado com sucesso.",
                IsSuccess = true
            };
        }
    }
}