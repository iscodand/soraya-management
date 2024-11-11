using Application.Dtos.User;
<<<<<<< HEAD
=======
using Application.Parameters;
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IUserService
    {
        public Task<Response<IEnumerable<GetUserDto>>> GetUsersByCompanyAsync(int companyId);
<<<<<<< HEAD
=======
        public Task<PagedResponse<IEnumerable<GetUserDto>>> GetUsersByCompanyPagedAsync(int companyId, RequestParameter parameter);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        public Task<Response<GetUserDto>> GetUserByUsernameAsync(string username);
        public Task<Response<DetailUserDto>> DetailUserAsync(string username, int companyId);
        public Task<Response<IEnumerable<GetRolesDto>>> GetRolesAsync();
        public Task<Response<UpdateUserDto>> UpdateUserAsync(UpdateUserDto updateUserDto);
        public Task<Response<GetUserDto>> ActivateUserAsync(string username, int companyId);
        public Task<Response<GetUserDto>> DeactivateUserAsync(string username, int companyId);
    }
}