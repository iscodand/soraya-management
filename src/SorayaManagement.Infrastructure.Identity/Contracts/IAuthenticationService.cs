using SorayaManagement.Infrastructure.Identity.Dtos;
using SorayaManagement.Infrastructure.Identity.Responses;

namespace SorayaManagement.Infrastructure.Identity.Contracts
{
    public interface IAuthenticationService
    {
        public Task<BaseResponse> RegisterAsync(RegisterUserDto registerUserDto);
        public Task<BaseResponse> LoginAsync(LoginUserDto loginUserDto);
        public Task<BaseResponse> LogoutAsync();
    }
}