using Microsoft.Extensions.DependencyInjection;
using Application.Contracts;
using Application.Services;

namespace Application
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDataService, DataService>();

            return services;
        }
    }
}