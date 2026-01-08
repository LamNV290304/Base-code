using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SEP490.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });



            services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
        }
    }
}
