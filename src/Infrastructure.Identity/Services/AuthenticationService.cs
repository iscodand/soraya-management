using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Infrastructure.Identity.Contracts;
using Infrastructure.Identity.Dtos;
using Infrastructure.Identity.Responses;

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

            if (!result.Succeeded)
            {
                return new BaseResponse()
                {
                    Message = "Ops. Um erro ocorreu no cadastro do usuário, verifique e tente novamente.",
                    Errors = result.Errors.Select(e => e.Description),
                    IsSuccess = false
                };
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