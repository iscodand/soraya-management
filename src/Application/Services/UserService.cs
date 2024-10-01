using Application.Dtos.User;
using Application.Responses;
using Domain.Entities;
using Application.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Application.Contracts.Services;
using Application.Dtos.Order;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository,
                           IRoleRepository roleRepository,
                           IUserRoleRepository userRoleRepository,
                           UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userManager = userManager;
        }

        public async Task<BaseResponse<IEnumerable<GetUserDto>>> GetUsersByCompanyAsync(int companyId)
        {
            ICollection<User> users = await _userRepository.GetUsersByCompanyAsync(companyId);
            var mappedUsers = GetUserDto.Map(users);

            return new()
            {
                Message = "Usuários encontrados com sucesso.",
                IsSuccess = true,
                Data = mappedUsers
            };
        }

        public async Task<BaseResponse<GetUserDto>> GetUserByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null)
            {
                return new()
                {
                    Message = "Usuário não encontrado.",
                    IsSuccess = false
                };
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            GetUserDto getUserDto = GetUserDto.Map(user, roles);

            return new()
            {
                Data = getUserDto,
                Message = "Usuário recuperado com sucesso.",
                IsSuccess = true
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
            // TODO => implementar paginação
            User user = await _userRepository.GetWithOrdersAsync(username);
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

            DetailUserDto detailUserDto = DetailUserDto.Map(user, role.Description);

            return new BaseResponse<DetailUserDto>()
            {
                Message = "Usuário encontrado com sucesso.",
                IsSuccess = true,
                Data = detailUserDto
            };
        }

        public async Task<BaseResponse<UpdateUserDto>> UpdateUserAsync(UpdateUserDto request)
        {
            User user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user is null)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            if (user.CompanyId != request.CompanyId)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            bool usernameAlreadyInUse = await _userRepository.UserExistsByUsernameAsync(request.Username, user.Id);
            if (usernameAlreadyInUse)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Esse nome de usuário já está sendo utilizado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }

            bool emailAlreadyInUse = await _userRepository.UserExistsByEmailAsync(request.Email, user.Id);
            if (emailAlreadyInUse)
            {
                return new BaseResponse<UpdateUserDto>()
                {
                    Message = "Esse e-mail já está sendo utilizado. Verifique e tente novamente.",
                    IsSuccess = false
                };
            }
;
            user = UpdateUserDto.Map(user, request);
            await _userRepository.UpdateAsync(user);

            return new()
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
    }
}