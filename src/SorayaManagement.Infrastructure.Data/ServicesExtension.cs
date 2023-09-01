using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Data.DataContext;
using SorayaManagement.Infrastructure.Data.Repositories;

namespace SorayaManagement.Infrastructure.Data
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging();
            });

            // Repositories Dependency Injection
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMealRepository, MealRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}