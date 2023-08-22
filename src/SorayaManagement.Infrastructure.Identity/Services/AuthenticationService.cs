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
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AuthenticationService(UserManager<User> userManager,
                                                       IAuthenticatedUserService authenticatedUserService)
        {
            _userManager = userManager;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<BaseResponse> RegisterAsync(RegisterUserDto registerUserDto)
        {
            string authenticatedUserName = _authenticatedUserService.GetAuthenticatedUserName();
            // Debug.WriteLine(authenticatedUserName);

            User authenticatedUser = await _userManager.FindByNameAsync(authenticatedUserName);

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

        public Task<BaseResponse> LoginAsync(LoginUserDto loginUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}