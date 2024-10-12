using Application.DTOs.Authentication;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IAuthenticationService
    {
        public Task<Response<string>> RegisterAsync(RegisterUserDto registerUserDto);
        public Task<Response<string>> LoginAsync(LoginUserDto loginUserDto);
        public Task<Response<string>> ForgotPasswordAsync(string email, string origin);
        public Task<Response<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        public Task<Response<string>> ChangePasswordAsync(string username, ChangePasswordDto changePasswordDto);
        public Task<Response<string>> LogoutAsync();
    }
}