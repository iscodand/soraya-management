using Domain.Entities;
using Infrastructure.Identity.Dtos;
using Infrastructure.Identity.Responses;

namespace Infrastructure.Identity.Contracts
{
    public interface IAuthenticationService
    {
        public Task<BaseResponse> RegisterAsync(RegisterUserDto registerUserDto);
        public Task<BaseResponse> LoginAsync(LoginUserDto loginUserDto);
        public Task<BaseResponse> LogoutAsync();
    }
}