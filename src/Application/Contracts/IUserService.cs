using Application.Dtos.User;
using Application.Responses;

namespace Application.Contracts
{
    public interface IUserService
    {
        public Task<BaseResponse<GetUserDto>> GetUsersByCompanyAsync(int companyId);
        public Task<BaseResponse<DetailUserDto>> DetailUserAsync(string username, int companyId);
        public Task<BaseResponse<GetRolesDto>> GetRolesAsync();
        public Task<BaseResponse<UpdateUserDto>> UpdateUserAsync(UpdateUserDto updateUserDto);
        public Task<BaseResponse<GetUserDto>> ActivateUserAsync(string username, int companyId);
        public Task<BaseResponse<GetUserDto>> DeactivateUserAsync(string username, int companyId);
        public Task<BaseResponse<GetUserDto>> DeleteUserAsync(string username, int companyId);
    }
}