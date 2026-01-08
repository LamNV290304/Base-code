using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SEP490.Infrastructure.Persistence;
using SEP490.Infrastructure.Repositories;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            //Service registrations for logging services can be added here
            services.AddScoped<ILoggingService, Logging>();

            return services;
        }
    }
}
