using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dto.User;
using SorayaManagement.Application.Dtos;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;

namespace SorayaManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<BaseResponse<GetUserDto>> GetUsersByCompanyAsync(int companyId)
        {
            ICollection<User> users = await _userRepository.GetUsersByCompanyAsync(companyId);

            List<GetUserDto> getUserDtoCollection = new();
            foreach (User user in users)
            {
                GetUserDto getUserDto = new()
                {
                    Name = user.Name,
                    Username = user.UserName,
                    Email = user.Email
                };

                getUserDtoCollection.Add(getUserDto);
            }

            return new BaseResponse<GetUserDto>()
            {
                Message = "Usuários encontrados com sucesso.",
                IsSuccess = true,
                DataCollection = getUserDtoCollection
            };
        }

        public async Task<BaseResponse<GetRolesDto>> GetRolesAsync()
        {
            ICollection<Role> roles = await _roleRepository.GetAllAsync();

            List<GetRolesDto> getRolesDtosCollection = new();
            foreach (Role role in roles)
            {
                GetRolesDto getRolesDto = new()
                {
                    Id = role.Id,
                    Description = role.Description
                };

                getRolesDtosCollection.Add(getRolesDto);
            }

            return new BaseResponse<GetRolesDto>()
            {
                Message = "Cargos encontrados com sucesso",
                IsSuccess = true,
                DataCollection = getRolesDtosCollection
            };
        }
    }
}