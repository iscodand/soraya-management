using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            await roleManager.CreateAsync(Role.Create(nameof(RoleEnum.SuperAdmin), "Super Administrador"));
            await roleManager.CreateAsync(Role.Create(nameof(RoleEnum.Admin), "Administrador"));
            await roleManager.CreateAsync(Role.Create(nameof(RoleEnum.Manager), "Gerente"));
            await roleManager.CreateAsync(Role.Create(nameof(RoleEnum.Employee), "Empregado"));
        }
    }
}