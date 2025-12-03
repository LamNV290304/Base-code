using SEP490.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SEP490.Infrastructure.Persistence;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.API
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly)
            );
        }
    }
}
