using Novus.Api.BackgroundServices;
using Novus.Api.IntegrationTests.Infrastructure.DataSeeders;
using Novus.Api.IntegrationTests.Infrastructure.Fakes;
using Novus.Core;
using Novus.Core.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace Novus.Api.IntegrationTests.Infrastructure
{
    internal class TestStartup(IConfiguration configuration)
        : Startup(configuration)
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()
                .AddMvcCore()
                .AddDataAnnotations();

            services.AddCoreComponents();
            services.AddSingleton<IPingService, FakePingService>();  //override registration with own fakes

            services.AddFeatureManagement();

            services.AddDbContext<EmployeesContext>(options =>
            {
                options.UseInMemoryDatabase("employees")
                    .UseSeeding(EmployeesDataSeeder.Seed)
                    .UseAsyncSeeding(EmployeesDataSeeder.SeedAsync);
            });
            services.AddDbContext<CarsContext>(options =>
            {
                options.UseInMemoryDatabase("cars")
                    .UseSeeding(CarsDataSeeder.Seed)
                    .UseAsyncSeeding(CarsDataSeeder.SeedAsync);
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var employees = app.ApplicationServices.GetRequiredService<EmployeesContext>();
            employees.Database.EnsureCreated();
            using var cars = app.ApplicationServices.GetRequiredService<CarsContext>();
            cars.Database.EnsureCreated();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
