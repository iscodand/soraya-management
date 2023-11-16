using Application.Contracts;
using Application.Dtos.User;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(IUserRepository userRepository,
                           IRoleRepository roleRepository,
                           IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
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
            foreach (Role role in roles.Where(x => x.Name != "Admin"))
            {
                GetRolesDto getRolesDto = new()
                {
                    Id = role.Id,
                    Name = role.Name,
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

        public async Task<BaseResponse<DetailUserDto>> DetailUserAsync(string username, int companyId)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return new BaseResponse<DetailUserDto>()
                {
                    Message = "Usuário não encontrado.",
                    IsSuccess = false
                };
            }

            if (user.CompanyId != companyId)
            {
                return new BaseResponse<DetailUserDto>()
                {
                    Message = "Esse usuário não faz parte da sua empresa.",
                    IsSuccess = false
                };
            }

            IdentityUserRole<string> userRole = await _userRoleRepository.GetUserRoleAsync(user.Id);
            Role role = await _roleRepository.GetRoleByIdAsync(userRole.RoleId);

            DetailUserDto detailUserDto = new()
            {
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                UserRole = role.Description
            };

            return new BaseResponse<DetailUserDto>()
            {
                Message = "Usuário encontrado com sucesso.",
                IsSuccess = true,
                Data = detailUserDto
            };
        }

        public async Task<BaseResponse<UpdateUserDto>> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Usuário não pode ser nulo. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            User user = await _userRepository.GetUserByUsernameAsync(updateUserDto.Username);

            if (user == null)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            if (user.CompanyId != updateUserDto.CompanyId)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            if (await _userRepository.UserExistsByUsernameAsync(updateUserDto.NewUsername))
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Esse nome de usuário já está sendo utilizado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            if (await _userRepository.UserExistsByEmailAsync(updateUserDto.NewEmail))
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Esse e-mail já está sendo utilizado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            user.Update(updateUserDto.Name, updateUserDto.NewEmail, updateUserDto.NewUsername);
            await _userRepository.UpdateAsync(user);

            return new BaseResponse<UpdateUserDto>()
            {
                Message = "Usuário atualizado com sucesso",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetUserDto>> ActivateUserAsync(string username, int companyId)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return new BaseResponse<GetUserDto>()
                {
                    Message = "Usuário não encontrado.",
                    IsSuccess = false
                };
            }

            if (user.CompanyId != companyId)
            {
                return new BaseResponse<GetUserDto>()
                {
                    Message = "Esse usuário não faz parte da sua empresa.",
                    IsSuccess = false
                };
            }

            if (user.IsActive)
            {
                return new BaseResponse<GetUserDto>()
                {
                    Message = "O usuário já está ativo.",
                    IsSuccess = false
                };
            }

            user.Activate();
            await _userRepository.UpdateAsync(user);

            return new BaseResponse<GetUserDto>()
            {
                Message = "Usuário foi ativo com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetUserDto>> DeactivateUserAsync(string username, int companyId)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return new BaseResponse<GetUserDto>()
                {
                    Message = "Usuário não encontrado.",
                    IsSuccess = false
                };
            }

            if (user.CompanyId != companyId)
            {
                return new BaseResponse<GetUserDto>()
                {
                    Message = "Esse usuário não faz parte da sua empresa.",
                    IsSuccess = false
                };
            }

            if (!user.IsActive)
            {
                return new BaseResponse<GetUserDto>()
                {
                    Message = "O usuário já está inativo.",
                    IsSuccess = false
                };
            }

            user.Deactivate();
            await _userRepository.UpdateAsync(user);

            return new BaseResponse<GetUserDto>()
            {
                Message = "Usuário foi desativado com sucesso.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse<GetUserDto>> DeleteUserAsync(string username, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}