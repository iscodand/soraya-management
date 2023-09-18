using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<User, IdentityRole>(options =>
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
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            // Set application login url path
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = new PathString("/auth/login");
                options.SlidingExpiration = true;
            });

            return services;
        }
    }
}