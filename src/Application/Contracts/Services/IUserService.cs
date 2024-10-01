using Application.Dtos.User;
using Application.Responses;

namespace Application.Contracts.Services
{
    public interface IUserService
    {
        public Task<BaseResponse<IEnumerable<GetUserDto>>> GetUsersByCompanyAsync(int companyId);
        public Task<BaseResponse<GetUserDto>> GetUserByUsernameAsync(string username);
        public Task<BaseResponse<DetailUserDto>> DetailUserAsync(string username, int companyId);
        public Task<BaseResponse<GetRolesDto>> GetRolesAsync();
        public Task<BaseResponse<UpdateUserDto>> UpdateUserAsync(UpdateUserDto updateUserDto);
        public Task<BaseResponse<GetUserDto>> ActivateUserAsync(string username, int companyId);
        public Task<BaseResponse<GetUserDto>> DeactivateUserAsync(string username, int companyId);
    }
}