using Application.Contracts;
using Application.Dtos.User;
using Application.Responses;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data.Contracts;

namespace Tests.Services
{
    public class UserServiceTest
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserServiceTest()
        {
            _userRepository = A.Fake<IUserRepository>();
            _roleRepository = A.Fake<IRoleRepository>();
            _userRoleRepository = A.Fake<IUserRoleRepository>();
            _userService = new UserService(_userRepository, _roleRepository, _userRoleRepository);
        }

        // Scenarios - Update User
        // 1° Valid Update
        // 2° Invalid Update - Existent E-mail
        // 3° Invalid Update - Existent Username
        // 4° Invalid Update - Null UpdateUserDto
        // 5° Invalid Update - Null User (not found user)
        // 6° Invalid Update - Try to update user from another company

        [Fact]
        public async Task UpdateUser_ValidUpdate_ReturnsSuccess()
        {
            // Arrange
            User user = A.Fake<User>();
            user.CompanyId = 1;
            user.UserName = "IscoTest";
            user.Email = "test@email.com";
            user.Name = "Testing name";

            UpdateUserDto updateUserDto = new()
            {
                Id = user.Id,
                Name = "New Name",
                Username = "username",
                NewUsername = "newUsername",
                NewEmail = "newEmail@email.com",
                CompanyId = 1
            };

            BaseResponse<UpdateUserDto> response = new()
            {
                Message = "Usuário atualizado com sucesso",
                IsSuccess = true
            };

            // Act
            A.CallTo(() => _userRepository.GetUserByUsernameAsync(updateUserDto.Username)).Returns(user);
            A.CallTo(() => _userRepository.UserExistsByUsernameAsync(updateUserDto.NewUsername)).Returns(false);
            A.CallTo(() => _userRepository.UserExistsByEmailAsync(updateUserDto.NewEmail)).Returns(false);
            A.CallTo(() => user.Update(updateUserDto.Name, updateUserDto.NewEmail, updateUserDto.NewUsername));
            A.CallTo(() => _userRepository.SaveAsync());

            var result = await _userService.UpdateUserAsync(updateUserDto);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUser_ExistentEmail_ReturnsError()
        {
            // Arrange
            User user = A.Fake<User>();
            user.CompanyId = 1;
            user.UserName = "IscoTest";
            user.Email = "test@email.com";
            user.Name = "Testing name";

            UpdateUserDto updateUserDto = new()
            {
                Id = user.Id,
                Name = "New Name",
                Username = "newIsco",
                NewEmail = "existentEmail@email.com",
                CompanyId = 1
            };

            BaseResponse<UpdateUserDto> response = new()
            {
                Message = "Esse e-mail já está sendo utilizado. Verifique e tente novamente.",
                IsSuccess = false
            };

            // Act
            A.CallTo(() => _userRepository.GetUserByUsernameAsync(updateUserDto.Username)).Returns(user);
            A.CallTo(() => _userRepository.UserExistsByUsernameAsync(updateUserDto.NewUsername)).Returns(false);
            A.CallTo(() => _userRepository.UserExistsByEmailAsync(updateUserDto.NewEmail)).Returns(true);

            var result = await _userService.UpdateUserAsync(updateUserDto);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUser_ExistentUsername_ReturnsError()
        {
            // Arrange
            User user = A.Fake<User>();
            user.CompanyId = 1;
            user.UserName = "IscoTest";
            user.Email = "test@email.com";
            user.Name = "Testing name";

            UpdateUserDto updateUserDto = new()
            {
                Id = user.Id,
                Name = "New Name",
                Username = "username",
                NewUsername = "existentUsername",
                NewEmail = "newEmail@email.com",
                CompanyId = 1
            };

            BaseResponse<UpdateUserDto> response = new()
            {
                Message = "Esse nome de usuário já está sendo utilizado. Verifique e tente novamente.",
                IsSuccess = false
            };

            // Act
            A.CallTo(() => _userRepository.GetUserByUsernameAsync(updateUserDto.Username)).Returns(user);
            A.CallTo(() => _userRepository.UserExistsByUsernameAsync(updateUserDto.NewUsername)).Returns(true);

            var result = await _userService.UpdateUserAsync(updateUserDto);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUser_NullUpdateUserDto_ReturnsError()
        {
            // Arrange
            UpdateUserDto? updateUserDto = null;

            BaseResponse<UpdateUserDto> response = new()
            {
                Message = "Usuário não pode ser nulo. Verifique e tente novamente.",
                IsSuccess = false
            };

            // Act
            var result = await _userService.UpdateUserAsync(updateUserDto);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUser_NotFoundUser_ReturnsError()
        {
            User? user = null;

            // Arrange
            UpdateUserDto? updateUserDto = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Novo Isco",
                Username = "username",
                NewUsername = "newUsername",
                NewEmail = "email@email.com",
                CompanyId = 1
            };

            BaseResponse<UpdateUserDto> response = new()
            {
                Message = "Usuário não encontrado. Verifique e tente novamente.",
                IsSuccess = false
            };

            // Act
            A.CallTo(() => _userRepository.GetUserByUsernameAsync("inexistentUser")).Returns(user);

            var result = await _userService.UpdateUserAsync(updateUserDto);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUser_UserFromAnotherCompany_ReturnsError()
        {
            // Arrange
            User user = A.Fake<User>();
            user.CompanyId = 1;
            user.UserName = "IscoTeste";
            user.Email = "teste@email.com";
            user.Name = "Testando Nome";

            UpdateUserDto updateUserDto = new()
            {
                Id = user.Id,
                Name = "Isco",
                Username = "username",
                NewUsername = "newUsername",
                NewEmail = "email@email.com",
                CompanyId = 2
            };

            BaseResponse<UpdateUserDto> response = new()
            {
                Message = "Usuário não encontrado. Verifique e tente novamente.",
                IsSuccess = false
            };

            // Act
            A.CallTo(() => _userRepository.GetUserByUsernameAsync(updateUserDto.Username)).Returns(user);

            var result = await _userService.UpdateUserAsync(updateUserDto);

            // Assert
            result.Should().BeEquivalentTo(response);
            result.Message.Should().Be(response.Message);
            result.IsSuccess.Should().Be(response.IsSuccess);
        }
    }
}