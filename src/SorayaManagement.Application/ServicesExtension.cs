using Microsoft.Extensions.DependencyInjection;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Services;

namespace SorayaManagement.Application
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}