using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection ResolveDbContextConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Development"));
            });

            return services;
        }
    }
}
