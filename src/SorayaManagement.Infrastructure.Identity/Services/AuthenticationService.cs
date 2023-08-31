using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Dtos;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AuthenticationService(UserManager<User> userManager,
                                     IAuthenticatedUserService authenticatedUserService,
                                     SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _authenticatedUserService = authenticatedUserService;
            _signInManager = signInManager;
        }

        public async Task<BaseResponse> RegisterAsync(RegisterUserDto registerUserDto)
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();

            if (registerUserDto == null)
            {
                return new BaseResponse()
                {
                    Message = "Usuário não pode ser nulo.",
                    IsSuccess = false
                };
            }

            User user = new()
            {
                Name = registerUserDto.Name,
                NormalizedName = registerUserDto.Name.Trim().ToUpper(),
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                CompanyId = authenticatedUser.CompanyId
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUserDto.Password);

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

        public Task<BaseResponse> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}