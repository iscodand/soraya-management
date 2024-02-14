using Application.DTOs.Authentication;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            RegisterUserDto registerUserDto = new()
            {
                Name = "Isco D'Andrade",
                Username = "iscodand",
                Email = "iscodand@email.com",
                Password = "pass123!",
                ConfirmPassword = "pass123!",
                CompanyId = 1,
                Role = nameof(RoleEnum.SuperAdmin)
            };

            bool userAlreadyRegistered = userManager.Users.Any(x => x.UserName == registerUserDto.Username);
            if (userAlreadyRegistered == false)
            {
                User user = User.Create(registerUserDto.Name,
                            registerUserDto.Email,
                            registerUserDto.Username,
                            registerUserDto.CompanyId);

                await userManager.CreateAsync(user, registerUserDto.Password);
                await userManager.AddToRoleAsync(user, registerUserDto.Role);
            }
        }
    }
}