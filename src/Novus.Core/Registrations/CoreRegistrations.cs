using Novus.Core.Providers;
using Novus.Core.Repositories;
using Novus.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Novus.Core.Registrations
{
    public static class CoreRegistrations
    {
        public static IServiceCollection AddCoreComponents(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICarService, CarService>();
            services.AddSingleton<VersionProvider>();

            return services;
        }
    }
}
