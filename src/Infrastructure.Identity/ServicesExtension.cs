using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Identity.Services;
using Application.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Identity
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddIdentitySetup(this IServiceCollection services)
        {
            // Dependency Injection
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddIdentity<User, Role>(options =>
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
                    // options.AccessDeniedPath = new PathString(""); => create a custom page for this
                    options.SlidingExpiration = true;
                });

            return services;
        }
    }
}