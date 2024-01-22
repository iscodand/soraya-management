using Application.DTOs.Authentication;
using Application.Responses;

namespace Application.Contracts
{
    public interface IAuthenticationService
    {
        public Task<BaseResponse<string>> RegisterAsync(RegisterUserDto registerUserDto);
        public Task<BaseResponse<string>> LoginAsync(LoginUserDto loginUserDto);
        public Task<BaseResponse<string>> ForgotPasswordAsync(string email, string origin);
        public Task<BaseResponse<string>> LogoutAsync();
    }
}