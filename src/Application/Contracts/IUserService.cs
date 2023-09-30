using SorayaManagement.Application.Dto.User;
using SorayaManagement.Application.Dtos;
using SorayaManagement.Application.Responses;

namespace SorayaManagement.Application.Contracts
{
    public interface IUserService
    {
        public Task<BaseResponse<GetUserDto>> GetUsersByCompanyAsync(int companyId);
        public Task<BaseResponse<GetRolesDto>> GetRolesAsync();
    }
}