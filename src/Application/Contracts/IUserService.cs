using Application.Dtos.User;
using Application.Responses;

namespace Application.Contracts
{
    public interface IUserService
    {
        public Task<BaseResponse<GetUserDto>> GetUsersByCompanyAsync(int companyId);
        public Task<BaseResponse<GetUserDto>> DetailUserAsync(string userId, int companyId);
        public Task<BaseResponse<GetRolesDto>> GetRolesAsync();
    }
}