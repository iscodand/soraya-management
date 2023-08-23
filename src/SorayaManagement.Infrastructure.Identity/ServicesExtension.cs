using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.DataContext;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Services;

namespace SorayaManagement.Infrastructure.Identity
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddIdentitySetup(this IServiceCollection services)
        {
            // Dependency Injection
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddIdentityCore<User>(options =>
            {
                // Password Options
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // User Options
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}