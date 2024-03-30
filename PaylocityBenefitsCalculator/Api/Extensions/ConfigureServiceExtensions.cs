using Api.Data;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;

namespace Api.Extensions
{
    public static class ConfigureServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            // Services
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IDependentService, DependentService>();
            services.AddTransient<IPaycheckService, PaycheckService>();

            // Repos
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IDependentRepository, DependentRepository>();
        }

        public static void ConfigureDb(this IServiceCollection services)
        {
            services.AddDbContext<PaylocityDbContext>();

            // Call EnsureCreated to create the database if it doesn't exist
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PaylocityDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
